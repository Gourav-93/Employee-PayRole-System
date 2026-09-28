using System.Security.Claims;
using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;
    private readonly IEmployeeService _employeeService;

    public AttendanceController(
        IAttendanceService service,
        IEmployeeService employeeService)
    {
        _service = service;
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var attendance = await _service.GetByIdAsync(id);

        if (attendance == null)
            return NotFound("Attendance record not found.");

        return Ok(attendance);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyAttendance()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return Unauthorized("Email claim not found.");

        var employee = await _employeeService
            .GetByEmailAsync(email);

        if (employee == null)
            return NotFound("Employee profile not found.");

        var attendance = await _service
            .GetByEmployeeIdAsync(employee.Id);

        return Ok(attendance);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Create(
        AttendanceCreateDto dto)
    {
        if (!Enum.TryParse<AttendanceStatus>(
            dto.Status,
            true,
            out var status))
        {
            return BadRequest("Invalid attendance status.");
        }

        var attendance = new Attendance
        {
            EmployeeId = dto.EmployeeId,
            Date = dto.Date,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            Status = status
        };

        return Ok(await _service.AddAsync(attendance));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        AttendanceUpdateDto dto)
    {
        var existing = await _service.GetByIdAsync(id);

        if (existing == null)
            return NotFound("Attendance record not found.");

        if (!Enum.TryParse<AttendanceStatus>(
            dto.Status,
            true,
            out var status))
        {
            return BadRequest("Invalid attendance status.");
        }

        existing.Date = dto.Date;
        existing.CheckIn = dto.CheckIn;
        existing.CheckOut = dto.CheckOut;
        existing.Status = status;

        return Ok(await _service.UpdateAsync(existing));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Attendance record not found.");

        return Ok("Attendance deleted successfully.");
    }
}
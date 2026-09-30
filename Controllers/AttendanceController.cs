using System.Security.Claims;
using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/attendance")]
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

    // Get all employee attendance 
    [HttpGet]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }   

    // Get attendance by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var attendance = await _service.GetByIdAsync(id);

        if (attendance == null)
            return NotFound("Attendance record not found.");

        return Ok(attendance);
    }

    // Get logged-in employee attendance -- its self
    [HttpGet("my-attendance")]
    [Authorize]
    public async Task<IActionResult> GetMyAttendance()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        var attendance = await _service.GetByEmployeeIdAsync(employee.Id);

        return Ok(attendance);
    }

    // Create attendance
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

        double? workingHours = dto.WorkingHours;
        if (!workingHours.HasValue && dto.CheckIn.HasValue && dto.CheckOut.HasValue)
        {
            workingHours = (dto.CheckOut.Value - dto.CheckIn.Value).TotalHours;
        }

        var attendance = new Attendance
        {
            EmployeeId = dto.EmployeeId,
            Date = dto.Date,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            WorkingHours = Math.Round(workingHours ?? 0, 2) > 0 ? Math.Round(workingHours ?? 0, 2) : null,
            Remarks = dto.Remarks,
            Status = status
        };

        return Ok(await _service.AddAsync(attendance));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(int id, AttendanceUpdateDto dto)
    {
        var attendance = await _service.GetByIdAsync(id);

        if (attendance == null)
            return NotFound("Attendance record not found.");

        if (!Enum.TryParse(dto.Status, true, out AttendanceStatus status))
            return BadRequest("Invalid attendance status.");

        attendance.Date = dto.Date;
        attendance.CheckIn = dto.CheckIn;
        attendance.CheckOut = dto.CheckOut;
        attendance.Remarks = dto.Remarks;
        attendance.Status = status;

        return Ok(await _service.UpdateAsync(attendance));
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

    [HttpGet("today")]
    [Authorize]
    public async Task<IActionResult> GetTodayAttendance()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        var attendance = await _service.GetByEmployeeAndDateAsync(
            employee.Id, DateTime.Today);

        return Ok(attendance);
    }

    [HttpPost("check-in")]
    [Authorize]
    public async Task<IActionResult> CheckIn()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        var existing = await _service.GetByEmployeeAndDateAsync(
            employee.Id, DateTime.Today);

        if (existing != null)
            return BadRequest("Attendance already marked for today.");

        var attendance = new Attendance
        {
            EmployeeId = employee.Id,
            Date = DateTime.Today,
            CheckIn = DateTime.Now.TimeOfDay,
            Status = AttendanceStatus.Present
        };

        return Ok(await _service.AddAsync(attendance));
    }

    [HttpPost("check-out")]
    [Authorize]
    public async Task<IActionResult> CheckOut()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        var attendance = await _service.GetByEmployeeAndDateAsync(
            employee.Id, DateTime.Today);

        if (attendance == null)
            return BadRequest("Please check in first.");
            
        if (attendance.CheckOut != null)
            return BadRequest("Attendance already checked out.");

        attendance.CheckOut = DateTime.Now.TimeOfDay;

        if (attendance.CheckIn.HasValue)
        {
            attendance.WorkingHours = Math.Round(
                (attendance.CheckOut.Value - attendance.CheckIn.Value).TotalHours, 2);
        }

        return Ok(await _service.UpdateAsync(attendance));
    }
}



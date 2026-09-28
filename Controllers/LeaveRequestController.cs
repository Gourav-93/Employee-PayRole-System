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
public class LeaveRequestController : ControllerBase
{
    private readonly ILeaveRequestService _service;
    private readonly IEmployeeService _employeeService;

    public LeaveRequestController(
        ILeaveRequestService service,
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
        var leave = await _service.GetByIdAsync(id);

        if (leave == null)
            return NotFound("Leave request not found.");

        return Ok(leave);
    }

    [HttpGet("me")]
    [Authorize(Roles = "EMPLOYEE")]
    public async Task<IActionResult> GetMyLeaves()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            return Unauthorized("User identity not found.");

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        return Ok(await _service
            .GetByEmployeeIdAsync(employee.Id));
    }

    [HttpPost]
    [Authorize(Roles = "EMPLOYEE")]
    public async Task<IActionResult> Create(
        LeaveRequestCreateDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            return Unauthorized("User identity not found.");

        var employee = await _employeeService.GetByUserIdAsync(userId);
        
        if (employee == null)
            return NotFound("Employee profile not found.");

        if (dto.ToDate < dto.FromDate)
            return BadRequest(new { message = "To date cannot be before from date." });

        var existingLeaves = await _service.GetByEmployeeIdAsync(employee.Id);
        var overlap = existingLeaves.Any(l => l.Status != LeaveStatus.Rejected &&
                                              l.FromDate.Date <= dto.ToDate.Date &&
                                              l.ToDate.Date >= dto.FromDate.Date);

        if (overlap)
            return BadRequest(new { message = "You already have a leave request for these dates." });

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = employee.Id,
            LeaveType = dto.LeaveType,
            FromDate = dto.FromDate,
            ToDate = dto.ToDate,
            Reason = dto.Reason,
            Status = LeaveStatus.Pending
        };

        return Ok(await _service.AddAsync(leaveRequest));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        LeaveRequestUpdateDto dto)
    {
        var existing = await _service.GetByIdAsync(id);

        if (existing == null)
            return NotFound("Leave request not found.");

        existing.LeaveType = dto.LeaveType;
        existing.FromDate = dto.FromDate;
        existing.ToDate = dto.ToDate;
        existing.Reason = dto.Reason;

        return Ok(await _service.UpdateAsync(existing));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Leave request not found.");

        return Ok("Leave request deleted successfully.");
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Approve(int id)
    {
        var leave = await _service.ApproveAsync(id);

        if (leave == null)
            return NotFound("Leave request not found.");

        return Ok(new
        {
            message = "Leave request approved successfully.",
            leave
        });
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Reject(int id)
    {
        var leave = await _service.RejectAsync(id);

        if (leave == null)
            return NotFound("Leave request not found.");

        return Ok(new
        {
            message = "Leave request rejected successfully.",
            leave
        });
    }
}
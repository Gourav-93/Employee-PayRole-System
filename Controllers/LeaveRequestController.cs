using System.Security.Claims;
using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/leave")]
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

    // Get all leaves
    [HttpGet]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }


    // Get leave by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var leave = await _service.GetByIdAsync(id);

        if (leave == null)
            return NotFound("Leave request not found.");

        return Ok(leave);
    }


    // Employee's own leaves
    [HttpGet("my-leaves")]
    [Authorize(Roles = "EMPLOYEE")]
    public async Task<IActionResult> GetMyLeaves()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        return Ok(await _service.GetByEmployeeIdAsync(employee.Id));
    }


    // Apply for leave
    [HttpPost]
    [Authorize(Roles = "EMPLOYEE")]
    public async Task<IActionResult> Create(LeaveRequestCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (dto.ToDate < dto.FromDate)
            return BadRequest("To date cannot be before from date.");

        var leaves = await _service.GetByEmployeeIdAsync(employee.Id);

        var overlap = leaves.Any(l =>
            l.Status != LeaveStatus.Rejected &&
            l.FromDate.Date <= dto.ToDate.Date &&
            l.ToDate.Date >= dto.FromDate.Date);

        if (overlap)
            return BadRequest("You already have a leave request for these dates.");

        var leave = new LeaveRequest
        {
            EmployeeId = employee.Id,
            LeaveType = dto.LeaveType,
            FromDate = dto.FromDate,
            ToDate = dto.ToDate,
            Reason = dto.Reason,
            Status = LeaveStatus.Pending
        };

        return Ok(await _service.AddAsync(leave));
    }


    // Update leave
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        LeaveRequestUpdateDto dto)
    {
        var leave = await _service.GetByIdAsync(id);

        if (leave == null)
            return NotFound("Leave request not found.");

        leave.LeaveType = dto.LeaveType;
        leave.FromDate = dto.FromDate;
        leave.ToDate = dto.ToDate;
        leave.Reason = dto.Reason;

        return Ok(await _service.UpdateAsync(leave));
    }


    // Delete leave
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id))
            return NotFound("Leave request not found.");

        return Ok("Leave request deleted successfully.");
    }


    // Approve leave
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


    // Reject leave
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
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestController : ControllerBase
{
    private readonly ILeaveRequestService _service;

    public LeaveRequestController(ILeaveRequestService service)
    {
        _service = service;
    }

    // GET: api/LeaveRequest
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var leaves = await _service.GetAllAsync();

        return Ok(leaves);
    }

    // GET: api/LeaveRequest/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var leave = await _service.GetByIdAsync(id);

        if (leave == null)
            return NotFound("Leave request not found.");

        return Ok(leave);
    }

    // POST: api/LeaveRequest
    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequest leaveRequest)
    {
        var createdLeave = await _service.AddAsync(leaveRequest);

        return Ok(createdLeave);
    }

    // PUT: api/LeaveRequest/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        LeaveRequest leaveRequest)
    {
        if (id != leaveRequest.Id)
            return BadRequest("Leave request ID mismatch.");

        var updatedLeave = await _service.UpdateAsync(leaveRequest);

        if (updatedLeave == null)
            return NotFound("Leave request not found.");

        return Ok(updatedLeave);
    }

    // DELETE: api/LeaveRequest/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Leave request not found.");

        return Ok("Leave request deleted successfully.");
    }
}
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service)
    {
        _service = service;
    }

    // GET: api/Attendance
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var attendance = await _service.GetAllAsync();

        return Ok(attendance);
    }

    // GET: api/Attendance/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var attendance = await _service.GetByIdAsync(id);

        if (attendance == null)
            return NotFound("Attendance record not found.");

        return Ok(attendance);
    }

    // POST: api/Attendance
    [HttpPost]
    public async Task<IActionResult> Create(Attendance attendance)
    {
        var createdAttendance = await _service.AddAsync(attendance);

        return Ok(createdAttendance);
    }

    // PUT: api/Attendance/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Attendance attendance)
    {
        if (id != attendance.Id)
            return BadRequest("Attendance ID mismatch.");

        var updatedAttendance = await _service.UpdateAsync(attendance);

        if (updatedAttendance == null)
            return NotFound("Attendance record not found.");

        return Ok(updatedAttendance);
    }

    // DELETE: api/Attendance/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Attendance record not found.");

        return Ok("Attendance deleted successfully.");
    }
}
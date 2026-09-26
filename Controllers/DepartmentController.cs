using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    // GET: api/Department
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _service.GetAllAsync();
        return Ok(departments);
    }

    // GET: api/Department/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _service.GetByIdAsync(id);

        if (department == null)
            return NotFound("Department not found.");

        return Ok(department);
    }

    // POST: api/Department
    [HttpPost]
    public async Task<IActionResult> Create(Department department)
    {
        var createdDepartment = await _service.AddAsync(department);

        return Ok(createdDepartment);
    }

    // PUT: api/Department/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Department department)
    {
        if (id != department.Id)
            return BadRequest("Department ID mismatch.");

        var updatedDepartment = await _service.UpdateAsync(department);

        if (updatedDepartment == null)
            return NotFound("Department not found.");

        return Ok(updatedDepartment);
    }

    // DELETE: api/Department/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Department not found.");

        return Ok("Department deleted successfully.");
    }
}
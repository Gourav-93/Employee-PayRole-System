using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN,HR")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _service.GetAllAsync();

        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _service.GetByIdAsync(id);

        if (department == null)
            return NotFound("Department not found.");

        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        DepartmentCreateDto dto)
    {
        var department = new Department
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var createdDepartment =
            await _service.AddAsync(department);

        return Ok(createdDepartment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        DepartmentUpdateDto dto)
    {
        var existingDepartment =
            await _service.GetByIdAsync(id);

        if (existingDepartment == null)
            return NotFound("Department not found.");

        existingDepartment.Name = dto.Name;
        existingDepartment.Description = dto.Description;

        var updatedDepartment =
            await _service.UpdateAsync(existingDepartment);

        return Ok(updatedDepartment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Department not found.");

        return Ok("Department deleted successfully.");
    }
}
using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _service.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _service.GetByIdAsync(id);

        if (employee == null)
            return NotFound("Employee not found.");

        return Ok(employee);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        var email = User.FindFirst(
            System.Security.Claims.ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
            return Unauthorized("Email claim not found.");

        var employee = await _service.GetByEmailAsync(email);

        if (employee == null)
            return NotFound("Employee profile not found.");

        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        var employee = new Employee
        {
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId,
            EmployeeCode = dto.EmployeeCode,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Designation = dto.Designation,
            BasicSalary = dto.BasicSalary,
            JoiningDate = dto.JoiningDate
        };

        var createdEmployee = await _service.AddAsync(employee);

        return Ok(createdEmployee);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        EmployeeUpdateDto dto)
    {
        var existingEmployee = await _service.GetByIdAsync(id);

        if (existingEmployee == null)
            return NotFound("Employee not found.");

        existingEmployee.DepartmentId = dto.DepartmentId;
        existingEmployee.EmployeeCode = dto.EmployeeCode;
        existingEmployee.Name = dto.Name;
        existingEmployee.Email = dto.Email;
        existingEmployee.Phone = dto.Phone;
        existingEmployee.Designation = dto.Designation;
        existingEmployee.BasicSalary = dto.BasicSalary;
        existingEmployee.JoiningDate = dto.JoiningDate;

        var updatedEmployee =
            await _service.UpdateAsync(existingEmployee);

        return Ok(updatedEmployee);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Employee not found.");

        return Ok("Employee deleted successfully.");
    }
}
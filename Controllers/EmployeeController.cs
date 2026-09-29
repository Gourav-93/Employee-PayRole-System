using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    // Get all employees
    [HttpGet]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }


    // Get employee by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _service.GetByIdAsync(id);

        if (employee == null)
            return NotFound("Employee not found.");

        return Ok(employee);
    }


    // Get logged-in employee profile
    [HttpGet("my-profile")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _service.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        return Ok(employee);
    }


    // Create employee
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

        return Ok(await _service.AddAsync(employee));
    }


    // Update employee
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        EmployeeUpdateDto dto)
    {
        var employee = await _service.GetByIdAsync(id);

        if (employee == null)
            return NotFound("Employee not found.");

        employee.DepartmentId = dto.DepartmentId;
        employee.EmployeeCode = dto.EmployeeCode;
        employee.Name = dto.Name;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.Designation = dto.Designation;
        employee.BasicSalary = dto.BasicSalary;
        employee.JoiningDate = dto.JoiningDate;

        return Ok(await _service.UpdateAsync(employee));
    }


    // Delete employee
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id))
            return NotFound("Employee not found.");

        return Ok("Employee deleted successfully.");
    }
}
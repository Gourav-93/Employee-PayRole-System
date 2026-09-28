using System.Security.Claims;
using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly IPayrollService _service;
    private readonly IEmployeeService _employeeService;

    public PayrollController(
        IPayrollService service,
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
        var payroll = await _service.GetByIdAsync(id);

        if (payroll == null)
            return NotFound("Payroll not found.");

        return Ok(payroll);
    }

    [HttpGet("me")]
    [Authorize(Roles = "EMPLOYEE")]
    public async Task<IActionResult> GetMyPayroll()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return Unauthorized("Email claim not found.");

        var employee = await _employeeService
            .GetByEmailAsync(email);

        if (employee == null)
            return NotFound("Employee profile not found.");

        return Ok(await _service
            .GetByEmployeeIdAsync(employee.Id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Create(
        PayrollCreateDto dto)
    {
        if (dto.Month < 1 || dto.Month > 12)
            return BadRequest("Month must be between 1 and 12.");

        if (dto.Year < 2000)
            return BadRequest("Invalid year.");

        var payroll = new Payroll
        {
            EmployeeId = dto.EmployeeId,
            Month = dto.Month,
            Year = dto.Year,
            BasicSalary = dto.BasicSalary,
            Allowances = dto.Allowances,
            Deductions = dto.Deductions,
            UnpaidLeaveDeduction = dto.UnpaidLeaveDeduction
        };

        return Ok(await _service.AddAsync(payroll));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        PayrollUpdateDto dto)
    {
        var existing = await _service.GetByIdAsync(id);

        if (existing == null)
            return NotFound("Payroll not found.");

        existing.Month = dto.Month;
        existing.Year = dto.Year;
        existing.BasicSalary = dto.BasicSalary;
        existing.Allowances = dto.Allowances;
        existing.Deductions = dto.Deductions;
        existing.UnpaidLeaveDeduction =
            dto.UnpaidLeaveDeduction;

        return Ok(await _service.UpdateAsync(existing));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Payroll not found.");

        return Ok("Payroll deleted successfully.");
    }
}
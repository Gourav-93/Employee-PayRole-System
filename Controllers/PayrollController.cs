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

    // Get all payroll
    [HttpGet]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }


    // Get payroll by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var payroll = await _service.GetByIdAsync(id);

        if (payroll == null)
            return NotFound("Payroll not found.");

        return Ok(payroll);
    }


    // Employee's own payroll
    [HttpGet("my-payroll")]
    [Authorize(Roles = "EMPLOYEE")]
    public async Task<IActionResult> GetMyPayroll()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var employee = await _employeeService.GetByUserIdAsync(userId);

        if (employee == null)
            return NotFound("Employee profile not found.");

        return Ok(await _service.GetByEmployeeIdAsync(employee.Id));
    }


    // Create payroll
    [HttpPost]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Create(PayrollCreateDto dto)
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


    // Update payroll
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Update(
        int id,
        PayrollUpdateDto dto)
    {
        var payroll = await _service.GetByIdAsync(id);

        if (payroll == null)
            return NotFound("Payroll not found.");

        payroll.Month = dto.Month;
        payroll.Year = dto.Year;
        payroll.BasicSalary = dto.BasicSalary;
        payroll.Allowances = dto.Allowances;
        payroll.Deductions = dto.Deductions;
        payroll.UnpaidLeaveDeduction = dto.UnpaidLeaveDeduction;

        return Ok(await _service.UpdateAsync(payroll));
    }


    // Delete payroll
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,HR")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id))
            return NotFound("Payroll not found.");

        return Ok("Payroll deleted successfully.");
    }
}
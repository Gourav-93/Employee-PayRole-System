using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly IPayrollService _service;

    public PayrollController(IPayrollService service)
    {
        _service = service;
    }

    // GET: api/Payroll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payrolls = await _service.GetAllAsync();

        return Ok(payrolls);
    }

    // GET: api/Payroll/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var payroll = await _service.GetByIdAsync(id);

        if (payroll == null)
            return NotFound("Payroll record not found.");

        return Ok(payroll);
    }

    // POST: api/Payroll
    [HttpPost]
    public async Task<IActionResult> Create(Payroll payroll)
    {
        var createdPayroll = await _service.AddAsync(payroll);

        return Ok(createdPayroll);
    }

    // PUT: api/Payroll/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Payroll payroll)
    {
        if (id != payroll.Id)
            return BadRequest("Payroll ID mismatch.");

        var updatedPayroll = await _service.UpdateAsync(payroll);

        if (updatedPayroll == null)
            return NotFound("Payroll record not found.");

        return Ok(updatedPayroll);
    }

    // DELETE: api/Payroll/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Payroll record not found.");

        return Ok("Payroll deleted successfully.");
    }
}
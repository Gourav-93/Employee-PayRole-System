using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
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

    // GET: api/Employee
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _service.GetAllAsync();
        return Ok(employees);
    }

    // GET: api/Employee/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _service.GetByIdAsync(id);

        if (employee == null)
            return NotFound("Employee not found.");

        return Ok(employee);
    }

    // POST: api/Employee
    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        var createdEmployee = await _service.AddAsync(employee);

        return Ok(createdEmployee);
    }

    // PUT: api/Employee/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        if (id != employee.Id)
            return BadRequest("Employee ID mismatch.");

        var updatedEmployee = await _service.UpdateAsync(employee);

        if (updatedEmployee == null)
            return NotFound("Employee not found.");

        return Ok(updatedEmployee);
    }

    // DELETE: api/Employee/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Employee not found.");

        return Ok("Employee deleted successfully.");
    }
}
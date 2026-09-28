using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN,HR")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _service.GetDashboardAsync();

        return Ok(dashboard);
    }
}
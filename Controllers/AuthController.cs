using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _service.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            return BadRequest("Email already registered.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            return BadRequest("Invalid role.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Role = role
        };

        var createdUser = await _service.RegisterAsync(user);

        return Ok(new
        {
            createdUser.Id,
            createdUser.Name,
            createdUser.Email,
            createdUser.Role
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _service.GetByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("Invalid email or password.");

        bool passwordValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!passwordValid)
            return Unauthorized("Invalid email or password.");

        return Ok(new
        {
            message = "Login successful.",
            user.Id,
            user.Name,
            user.Email,
            user.Role
        });
    }
}
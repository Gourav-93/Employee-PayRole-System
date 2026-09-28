using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementPayrollSystem.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IEmployeeService _employeeService;

    public AuthController(
        IUserService userService,
        IJwtService jwtService,
        IEmployeeService employeeService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _employeeService = employeeService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser =
            await _userService.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            return BadRequest("Email already registered.");

        if (!Enum.TryParse<UserRole>(
            dto.Role,
            true,
            out var role))
        {
            return BadRequest("Invalid role.");
        }

        if (role == UserRole.EMPLOYEE)
            return BadRequest("Please use /register-employee to register an employee.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Role = role
        };

        var createdUser =
            await _userService.RegisterAsync(user);

        var response = new UserResponseDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            Phone = createdUser.Phone,
            Role = createdUser.Role.ToString()
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user =
            await _userService.GetByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("Invalid email or password.");

        bool passwordValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!passwordValid)
            return Unauthorized("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        var response = new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString()
        };

        return Ok(new
        {
            message = "Login successful.",
            token,
            user = response
        });
    }

    [HttpPost("register-employee")]
    public async Task<IActionResult> RegisterEmployee(RegisterEmployeeDto dto)
    {
        var existingUser = await _userService.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest("Email already registered.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Role = UserRole.EMPLOYEE
        };

        var createdUser = await _userService.RegisterAsync(user);

        var employee = new Employee
        {
            UserId = createdUser.Id,
            DepartmentId = dto.DepartmentId,
            EmployeeCode = dto.EmployeeCode,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Designation = dto.Designation,
            BasicSalary = dto.BasicSalary,
            JoiningDate = dto.JoiningDate
        };

        await _employeeService.AddAsync(employee);

        var response = new UserResponseDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            Phone = createdUser.Phone,
            Role = createdUser.Role.ToString()
        };

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            return Unauthorized("User identity not found.");

        var user = await _userService.GetByIdAsync(userId);
        if (user == null) return NotFound("User not found.");

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString()
        });
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe(UserUpdateProfileDto dto)
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            return Unauthorized("User identity not found.");

        var user = await _userService.GetByIdAsync(userId);
        if (user == null) return NotFound("User not found.");

        user.Name = dto.Name;
        user.Phone = dto.Phone;

        var updatedUser = await _userService.UpdateAsync(user);

        return Ok(new UserResponseDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Email = updatedUser.Email,
            Phone = updatedUser.Phone,
            Role = updatedUser.Role.ToString()
        });
    }
}
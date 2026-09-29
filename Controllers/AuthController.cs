using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    // Register Admin or HR
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var userExists = await _userService.GetByEmailAsync(dto.Email);

        if (userExists != null)
            return BadRequest("Email already registered.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            return BadRequest("Invalid role.");

        if (role == UserRole.EMPLOYEE)
            return BadRequest("Use register-employee for employees.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Role = role
        };

        var createdUser = await _userService.RegisterAsync(user);

        return Ok(new UserResponseDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            Phone = createdUser.Phone,
            Role = createdUser.Role.ToString()
        });
    }


    // Login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userService.GetByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            message = "Login successful.",
            token,
            user = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role.ToString()
            }
        });
    }


    // Register Employee
    [HttpPost("register-employee")]
    public async Task<IActionResult> RegisterEmployee(RegisterEmployeeDto dto)
    {
        var userExists = await _userService.GetByEmailAsync(dto.Email);

        if (userExists != null)
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

        return Ok(new UserResponseDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            Phone = createdUser.Phone,
            Role = createdUser.Role.ToString()
        });
    }


    // Get Logged-in User
    [HttpGet("my-profile")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString()
        });
    }


    // Update Logged-in User
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe(UserUpdateProfileDto dto)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

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
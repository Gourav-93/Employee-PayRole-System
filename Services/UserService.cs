using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using BCrypt.Net;

namespace EmployeeManagementPayrollSystem.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _repository.GetByEmailAsync(email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<User> RegisterAsync(User user)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        return await _repository.AddAsync(user);
    }

    public async Task<User> UpdateAsync(User user)
    {
        return await _repository.UpdateAsync(user);
    }
}
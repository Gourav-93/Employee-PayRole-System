using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<User> RegisterAsync(User user);
    Task<User> UpdateAsync(User user);
}
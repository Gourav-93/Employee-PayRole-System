using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
}
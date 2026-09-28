using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
}
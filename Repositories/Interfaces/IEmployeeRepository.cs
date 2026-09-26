using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();

    Task<Employee?> GetByIdAsync(int id);

    Task<Employee> AddAsync(Employee employee);

    Task<Employee?> UpdateAsync(Employee employee);

    Task<bool> DeleteAsync(int id);
}
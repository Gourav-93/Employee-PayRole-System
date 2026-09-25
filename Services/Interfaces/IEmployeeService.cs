using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee?> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
}
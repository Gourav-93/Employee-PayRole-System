using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department> AddAsync(Department department);
    Task<Department?> UpdateAsync(Department department);
    Task<bool> DeleteAsync(int id);
}
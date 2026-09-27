using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Repositories.Interfaces;

public interface IPayrollRepository
{
    Task<List<Payroll>> GetAllAsync();
    Task<Payroll?> GetByIdAsync(int id);
    Task<Payroll> AddAsync(Payroll payroll);
    Task<Payroll?> UpdateAsync(Payroll payroll);
    Task<bool> DeleteAsync(int id);
}
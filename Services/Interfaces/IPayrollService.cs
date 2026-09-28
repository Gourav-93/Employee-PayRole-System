using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface IPayrollService
{
    Task<List<Payroll>> GetAllAsync();
    Task<Payroll?> GetByIdAsync(int id);
    Task<List<Payroll>> GetByEmployeeIdAsync(int employeeId);
    Task<Payroll> AddAsync(Payroll payroll);
    Task<Payroll?> UpdateAsync(Payroll payroll);
    Task<bool> DeleteAsync(int id);
}
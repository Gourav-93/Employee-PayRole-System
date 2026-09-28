using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using EmployeeManagementPayrollSystem.Services.Interfaces;

namespace EmployeeManagementPayrollSystem.Services;

public class PayrollService : IPayrollService
{
    private readonly IPayrollRepository _repository;

    public PayrollService(IPayrollRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Payroll>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Payroll?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<Payroll>> GetByEmployeeIdAsync(
        int employeeId)
    {
        return await _repository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<Payroll> AddAsync(Payroll payroll)
    {
        payroll.NetSalary =
            payroll.BasicSalary
            + payroll.Allowances
            - payroll.Deductions
            - payroll.UnpaidLeaveDeduction;

        payroll.GeneratedDate = DateTime.UtcNow;

        return await _repository.AddAsync(payroll);
    }

    public async Task<Payroll?> UpdateAsync(Payroll payroll)
    {
        payroll.NetSalary =
            payroll.BasicSalary
            + payroll.Allowances
            - payroll.Deductions
            - payroll.UnpaidLeaveDeduction;

        return await _repository.UpdateAsync(payroll);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
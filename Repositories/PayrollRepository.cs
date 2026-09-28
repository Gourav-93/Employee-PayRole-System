using EmployeeManagementPayrollSystem.Data;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementPayrollSystem.Repositories;

public class PayrollRepository : IPayrollRepository
{
    private readonly AppDbContext _context;

    public PayrollRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Payroll>> GetAllAsync()
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync();
    }

    public async Task<Payroll?> GetByIdAsync(int id)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Payroll>> GetByEmployeeIdAsync(
        int employeeId)
    {
        return await _context.Payrolls
            .Where(p => p.EmployeeId == employeeId)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync();
    }

    public async Task<Payroll> AddAsync(Payroll payroll)
    {
        _context.Payrolls.Add(payroll);

        await _context.SaveChangesAsync();

        return payroll;
    }

    public async Task<Payroll?> UpdateAsync(Payroll payroll)
    {
        var existing = await _context.Payrolls
            .FindAsync(payroll.Id);

        if (existing == null)
            return null;

        existing.EmployeeId = payroll.EmployeeId;
        existing.Month = payroll.Month;
        existing.Year = payroll.Year;
        existing.BasicSalary = payroll.BasicSalary;
        existing.Allowances = payroll.Allowances;
        existing.Deductions = payroll.Deductions;
        existing.UnpaidLeaveDeduction =
            payroll.UnpaidLeaveDeduction;
        existing.NetSalary = payroll.NetSalary;
        existing.GeneratedDate = payroll.GeneratedDate;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var payroll = await _context.Payrolls
            .FindAsync(id);

        if (payroll == null)
            return false;

        _context.Payrolls.Remove(payroll);

        await _context.SaveChangesAsync();

        return true;
    }
}
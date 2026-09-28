using EmployeeManagementPayrollSystem.Data;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementPayrollSystem.Repositories;

public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly AppDbContext _context;

    public LeaveRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaveRequest>> GetAllAsync()
    {
        return await _context.LeaveRequests
            .Include(l => l.Employee)
            .ToListAsync();
    }

    public async Task<LeaveRequest?> GetByIdAsync(int id)
    {
        return await _context.LeaveRequests
            .Include(l => l.Employee)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<List<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.LeaveRequests
            .Where(l => l.EmployeeId == employeeId)
            .OrderByDescending(l => l.FromDate)
            .ToListAsync();
    }

    public async Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest)
    {
        _context.LeaveRequests.Add(leaveRequest);
        await _context.SaveChangesAsync();

        return leaveRequest;
    }

    public async Task<LeaveRequest?> UpdateAsync(
        LeaveRequest leaveRequest)
    {
        var existing = await _context.LeaveRequests
            .FindAsync(leaveRequest.Id);

        if (existing == null)
            return null;

        existing.EmployeeId = leaveRequest.EmployeeId;
        existing.LeaveType = leaveRequest.LeaveType;
        existing.FromDate = leaveRequest.FromDate;
        existing.ToDate = leaveRequest.ToDate;
        existing.Reason = leaveRequest.Reason;
        existing.Status = leaveRequest.Status;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var leaveRequest = await _context.LeaveRequests
            .FindAsync(id);

        if (leaveRequest == null)
            return false;

        _context.LeaveRequests.Remove(leaveRequest);
        await _context.SaveChangesAsync();

        return true;
    }
}
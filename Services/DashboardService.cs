using EmployeeManagementPayrollSystem.Data;
using EmployeeManagementPayrollSystem.DTOs;
using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementPayrollSystem.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var today = DateTime.Today;

        return new DashboardDto
        {
            TotalEmployees = await _context.Employees.CountAsync(),

            TotalDepartments = await _context.Departments.CountAsync(),

            PresentToday = await _context.Attendances
                    .CountAsync(a =>
                        a.Date.Date == today &&
                        a.Status == AttendanceStatus.Present),

            AbsentToday =
                await _context.Attendances
                    .CountAsync(a =>
                        a.Date.Date == today &&
                        a.Status == AttendanceStatus.Absent),

            PendingLeaves =
                await _context.LeaveRequests
                    .CountAsync(l =>
                        l.Status == LeaveStatus.Pending),

            ApprovedLeaves =
                await _context.LeaveRequests
                    .CountAsync(l =>
                        l.Status == LeaveStatus.Approved),

            TotalPayrollRecords =
                await _context.Payrolls.CountAsync()
        };
    }
}
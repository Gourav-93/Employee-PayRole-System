using EmployeeManagementPayrollSystem.DTOs;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
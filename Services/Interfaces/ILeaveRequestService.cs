using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface ILeaveRequestService
{
    Task<List<LeaveRequest>> GetAllAsync();
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest);
    Task<LeaveRequest?> UpdateAsync(LeaveRequest leaveRequest);
    Task<bool> DeleteAsync(int id);
}
using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface ILeaveRequestService
{
    Task<List<LeaveRequest>> GetAllAsync();
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task<List<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
    Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest);
    Task<LeaveRequest?> UpdateAsync(LeaveRequest leaveRequest);
    Task<bool> DeleteAsync(int id);

    Task<LeaveRequest?> ApproveAsync(int id);
    Task<LeaveRequest?> RejectAsync(int id);
}
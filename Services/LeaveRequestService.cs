using EmployeeManagementPayrollSystem.Enums;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using EmployeeManagementPayrollSystem.Services.Interfaces;

namespace EmployeeManagementPayrollSystem.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestRepository _repository;

    public LeaveRequestService(ILeaveRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LeaveRequest>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<LeaveRequest?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<LeaveRequest>> GetByEmployeeIdAsync(
        int employeeId)
    {
        return await _repository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<LeaveRequest> AddAsync(
        LeaveRequest leaveRequest)
    {
        leaveRequest.Status = LeaveStatus.Pending;

        return await _repository.AddAsync(leaveRequest);
    }

    public async Task<LeaveRequest?> UpdateAsync(
        LeaveRequest leaveRequest)
    {
        return await _repository.UpdateAsync(leaveRequest);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<LeaveRequest?> ApproveAsync(int id)
    {
        var leave = await _repository.GetByIdAsync(id);

        if (leave == null)
            return null;

        leave.Status = LeaveStatus.Approved;

        return await _repository.UpdateAsync(leave);
    }

    public async Task<LeaveRequest?> RejectAsync(int id)
    {
        var leave = await _repository.GetByIdAsync(id);

        if (leave == null)
            return null;

        leave.Status = LeaveStatus.Rejected;

        return await _repository.UpdateAsync(leave);
    }
}
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

    public async Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest)
    {
        return await _repository.AddAsync(leaveRequest);
    }

    public async Task<LeaveRequest?> UpdateAsync(LeaveRequest leaveRequest)
    {
        return await _repository.UpdateAsync(leaveRequest);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
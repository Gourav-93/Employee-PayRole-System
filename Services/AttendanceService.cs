using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using EmployeeManagementPayrollSystem.Services.Interfaces;

namespace EmployeeManagementPayrollSystem.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repository;

    public AttendanceService(IAttendanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Attendance>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Attendance> AddAsync(Attendance attendance)
    {
        return await _repository.AddAsync(attendance);
    }

    public async Task<Attendance?> UpdateAsync(Attendance attendance)
    {
        return await _repository.UpdateAsync(attendance);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
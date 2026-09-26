using EmployeeManagementPayrollSystem.Models;

namespace EmployeeManagementPayrollSystem.Services.Interfaces;

public interface IAttendanceService
{
    Task<List<Attendance>> GetAllAsync();
    Task<Attendance?> GetByIdAsync(int id);
    Task<Attendance> AddAsync(Attendance attendance);
    Task<Attendance?> UpdateAsync(Attendance attendance);
    Task<bool> DeleteAsync(int id);
}
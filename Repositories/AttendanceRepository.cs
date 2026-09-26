using EmployeeManagementPayrollSystem.Data;
using EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementPayrollSystem.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _context;

    public AttendanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Attendance>> GetAllAsync()
    {
        return await _context.Attendances
            .Include(a => a.Employee)
            .ToListAsync();
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Attendance> AddAsync(Attendance attendance)
    {
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        return attendance;
    }

    public async Task<Attendance?> UpdateAsync(Attendance attendance)
    {
        var existing = await _context.Attendances.FindAsync(attendance.Id);

        if (existing == null)
            return null;

        existing.EmployeeId = attendance.EmployeeId;
        existing.Date = attendance.Date;
        existing.CheckIn = attendance.CheckIn;
        existing.CheckOut = attendance.CheckOut;
        existing.Status = attendance.Status;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);

        if (attendance == null)
            return false;

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();

        return true;
    }
}
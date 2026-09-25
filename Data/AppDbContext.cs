using EmployeeManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementPayrollSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Attendance> Attendances { get; set; }

    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    public DbSet<Payroll> Payrolls { get; set; }
}
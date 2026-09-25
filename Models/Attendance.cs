namespace EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Enums;
public class Attendance
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan? CheckIn { get; set; }

    public TimeSpan? CheckOut { get; set; }

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

    // Relationship
    public Employee Employee { get; set; } = null!;
}
namespace EmployeeManagementPayrollSystem.Models;
using EmployeeManagementPayrollSystem.Enums;
public class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; } = string.Empty;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    // Relationship
    public Employee Employee { get; set; } = null!;
}
namespace EmployeeManagementPayrollSystem.DTOs;

public class LeaveRequestCreateDto
{
    public int EmployeeId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class LeaveRequestUpdateDto
{
    public string LeaveType { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}
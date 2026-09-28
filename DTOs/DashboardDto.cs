namespace EmployeeManagementPayrollSystem.DTOs;

public class DashboardDto
{
    public int TotalEmployees { get; set; }
    public int TotalDepartments { get; set; }
    public int PresentToday { get; set; }
    public int AbsentToday { get; set; }
    public int PendingLeaves { get; set; }
    public int ApprovedLeaves { get; set; }
    public int TotalPayrollRecords { get; set; }
}
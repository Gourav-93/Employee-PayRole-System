namespace EmployeeManagementPayrollSystem.DTOs;

public class AttendanceCreateDto
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? CheckIn { get; set; }
    public TimeSpan? CheckOut { get; set; }
    public string Status { get; set; } = "Present";
}

public class AttendanceUpdateDto
{
    public DateTime Date { get; set; }
    public TimeSpan? CheckIn { get; set; }
    public TimeSpan? CheckOut { get; set; }
    public string Status { get; set; } = "Present";
}
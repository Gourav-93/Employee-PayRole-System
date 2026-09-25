namespace EmployeeManagementPayrollSystem.Models;

public class Employee
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DepartmentId { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public decimal BasicSalary { get; set; }

    public DateTime JoiningDate { get; set; }

    // Relationships
    public User User { get; set; } = null!;

    public Department Department { get; set; } = null!;

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}
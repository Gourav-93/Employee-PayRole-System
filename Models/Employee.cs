using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementPayrollSystem.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(20)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Designation { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal BasicSalary { get; set; }

    [Required]
    public DateTime JoiningDate { get; set; }

    public User User { get; set; } = null!;
    public Department Department { get; set; } = null!;

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

    public ICollection<LeaveRequest> LeaveRequests { get; set; }
        = new List<LeaveRequest>();

    public ICollection<Payroll> Payrolls { get; set; }
        = new List<Payroll>();
}
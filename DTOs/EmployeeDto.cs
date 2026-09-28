namespace EmployeeManagementPayrollSystem.DTOs;

public class EmployeeCreateDto
{
    public int UserId { get; set; }
    public int DepartmentId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
    public DateTime JoiningDate { get; set; }
}

public class EmployeeUpdateDto
{
    public int DepartmentId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
    public DateTime JoiningDate { get; set; }
}
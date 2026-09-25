namespace EmployeeManagementPayrollSystem.Models;

public class Payroll
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal BasicSalary { get; set; }

    public decimal Allowances { get; set; }

    public decimal Deductions { get; set; }

    public decimal UnpaidLeaveDeduction { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime GeneratedDate { get; set; }

    // Relationship
    public Employee Employee { get; set; } = null!;
}
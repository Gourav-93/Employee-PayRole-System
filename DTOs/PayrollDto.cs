namespace EmployeeManagementPayrollSystem.DTOs;

public class PayrollCreateDto
{
    public int EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
    public decimal UnpaidLeaveDeduction { get; set; }
}

public class PayrollUpdateDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
    public decimal UnpaidLeaveDeduction { get; set; }
}
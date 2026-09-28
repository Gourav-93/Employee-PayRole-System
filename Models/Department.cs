using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementPayrollSystem.Models;

public class Department
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string Description { get; set; } = string.Empty;

    public ICollection<Employee> Employees { get; set; }
        = new List<Employee>();
}
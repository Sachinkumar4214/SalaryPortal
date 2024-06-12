using System.ComponentModel.DataAnnotations;

namespace SalaryPortal.Models
{
    public class EmployeeViewModel
    {
        [Key]
        public int? Salary_Id { get; set; }
        public double? Gross_Salary { get; set; }
        public double? Medical_Allowance { get; set; }
        public double? Basic_Salary { get; set; }
        public double? HRA { get; set; }
        public int? Employee_Id { get; set; }
    }
}

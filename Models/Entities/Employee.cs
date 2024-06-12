using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace SalaryPortal.Models.Entities
{
    public class Employee
    {
        [Key]
        public int Employee_Id {  get; set; }
        public ICollection<Salary> Salaries { get; set; }
    }
}

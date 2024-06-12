using Microsoft.EntityFrameworkCore;
using SalaryPortal.Models.Entities;

namespace SalaryPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Salary> Salary { get; set; }
        public DbSet<Employee>Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salary>().HasKey(x => x.Salary_Id);
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Salary and Employee entities
            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Salaries)
                .HasForeignKey(s => s.Employee_Id)
                .OnDelete(DeleteBehavior.Restrict); // Or your preferred delete behavior
        }
    }
}

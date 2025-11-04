using Microsoft.EntityFrameworkCore;

using EmployeeManagement.Domain;
namespace EmployeeManagement.Persistence
{
    public class EmployeeManagementDbContext : DbContext
    {
        public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);
            

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Employee> Employees
        {
            get; set;
        }
    }
}

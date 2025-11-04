using EmployeeManagement.Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
        public string? Position { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        [PastOrTodayDate(ErrorMessage = "Date of hire must be today or past")]
        public DateTime DateHired { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department? AssignedDepartment { get; set; }
    }
}

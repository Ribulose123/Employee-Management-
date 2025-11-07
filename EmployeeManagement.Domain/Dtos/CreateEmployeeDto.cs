using EmployeeManagement.Domain;
using EmployeeManagement.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Dtos
{
    public class CreateEmployeeDto
    {
        public string? Name { get; set; }
        public string? Position { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        [PastOrTodayDate(ErrorMessage = "Date of hire must be today or past")]
        public DateTime DateHired { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}

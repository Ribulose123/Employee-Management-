using EmployeeManagement.Domain;
using EmployeeManagement.Domain.Dtos;

namespace EmployeeManagement.API.Interfaces
{
    public interface IEmployeeServices
    {
        Task<(bool Success, string Message, Employee? Employee)> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<(bool Success, string Message, Employee?)> UpdateEmployeeAsync(int id, UpdateEmployeeDepartmentDto dto);
        Task<List<Employee>> GetEmployeesAsync();
        Task<(bool Success, string Message)> DeleteEmployeeAsync(int id);
    }
}

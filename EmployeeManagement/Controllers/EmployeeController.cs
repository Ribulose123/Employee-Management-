using EmployeeManagement.API.Interfaces;
using EmployeeManagement.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeServices _employeeService;

    public EmployeeController(IEmployeeServices employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
    {
        var result = await _employeeService.CreateEmployeeAsync(dto);
        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return CreatedAtAction(nameof(GetEmployeeById), new { id = result.Employee!.Id }, result.Employee);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employees = await _employeeService.GetEmployeesAsync();
        var employee = employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);
        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}

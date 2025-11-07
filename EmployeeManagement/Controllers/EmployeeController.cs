using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain;
using EmployeeManagement.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public EmployeeController(EmployeeManagementDbContext context)
        {

            _context = context;
        }

        // Create an Employee
        [HttpPost]
        public async Task<ActionResult <Employee>> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            if (dto == null)
                return BadRequest();

            var departmentExist = await _context.Departments
     .AnyAsync(d => d.Id == dto.DepartmentId && !d.isDeleted);


            if (!departmentExist)
            {
                var unassigned = await _context.Departments.FirstOrDefaultAsync(e => e.DepartmentName == "Unassigned");

                if (unassigned == null)
                {
                    unassigned = new Department { DepartmentName = "Unassigned" };
                    await _context.Departments.AddAsync(unassigned);
                    await _context.SaveChangesAsync();
                }
                dto.DepartmentId = unassigned.Id;
            }

            var employee = new Employee
            {
                Name = dto.Name,
                Position = dto.Position,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                DateHired = dto.DateHired,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            var result = await _context.Employees.Include(s => s.AssignedDepartment).FirstOrDefaultAsync(s => s.Id == employee.Id);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, result);
        }
        

        //Get Employee by id
        [HttpGet ("{id}")]

        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.Include(s=>s.AssignedDepartment).FirstOrDefaultAsync(s => s.Id == id);

            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        //Get All Employee
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            return await _context.Employees.Include(s => s.AssignedDepartment).ToListAsync();
        }

        //Update Assigen Department

        [HttpPatch ("{id}/department")]

        public async Task<ActionResult<Employee>> UpdateDepartmentId(int id, [FromBody] UpdateEmployeeDepartmentDto dto)
        {
            var employeeExist = await _context.Employees.FindAsync(id);

            if (employeeExist== null)
                return NotFound();

            var departmentExist = await _context.Departments.FindAsync(dto.DepartmentId);
            if (departmentExist == null)
                return NotFound(new { message = "Department not found" });


            employeeExist.DepartmentId = dto.DepartmentId;

            
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Employee '{employeeExist.Name}' successfully assigned to '{departmentExist.DepartmentName}' department."
            });
        }

        //Detele 

        [HttpDelete]
        public async Task <IActionResult> DeleteEmployee(int id)
        {
            var employeeExit = await _context.Employees.FindAsync(id);

            if (employeeExit == null)
                return NotFound();

            _context.Employees.Remove(employeeExit);
            await _context.SaveChangesAsync();

            return Ok();
        }
       

    }
}

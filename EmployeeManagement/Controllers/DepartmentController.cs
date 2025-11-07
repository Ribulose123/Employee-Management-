using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Persistence;
using EmployeeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EmployeeManagement.Domain.Dtos;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentController(EmployeeManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Create a department
        [HttpPost]

        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            if (department == null)
                return BadRequest();

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return Ok(department);
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartmentDto>>> GetAllDepartment()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .ToListAsync();

            var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

            return Ok(departmentDtos);
        }





        [HttpPatch ("{id}")]

        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            var departmentExit = await _context.Departments.FindAsync(id);

            if (departmentExit == null)
                return NotFound("No department found");

            departmentExit.DepartmentName = department.DepartmentName;

            await _context.SaveChangesAsync();
            return Ok($"{departmentExit.DepartmentName} successfully update");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var departmentExist = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (departmentExist == null)
                return NotFound();

            //Check if department has be deleted

            if (departmentExist.isDeleted)
            {
                return NotFound("Department has been deleted");
            }

            // Check for backup department
            var backup = await _context.Departments
                .FirstOrDefaultAsync(s => s.DepartmentName == "Unassigned");

            // If backup doesn't exist, create it
            if (backup == null)
            {
                backup = new Department { DepartmentName = "Unassigned" };
                await _context.Departments.AddAsync(backup);
                await _context.SaveChangesAsync();
            }

            // Reassign employees to backup
            if (departmentExist.Employees != null && departmentExist.Employees.Any())
            {
                foreach (var emp in departmentExist.Employees)
                {
                    emp.DepartmentId = backup.Id;
                }
            }

            // Soft delete
            departmentExist.isDeleted = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }



    }
}

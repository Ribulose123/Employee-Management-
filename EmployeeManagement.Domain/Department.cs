using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }

        public bool isDeleted { get; set; } = false;

        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
    }
}

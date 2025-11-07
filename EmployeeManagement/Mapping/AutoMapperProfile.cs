using AutoMapper;
using EmployeeManagement.Domain;
using EmployeeManagement.Domain.Dtos;

namespace EmployeeManagement.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
        }
    }
}

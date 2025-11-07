using AutoMapper;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Mapping;
using EmployeeManagement.API.MiddleWare;
using EmployeeManagement.API.Services;
using EmployeeManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//Add automapper
builder.Services.
    AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddScoped<IDepartmentService, DepartmentService>();


// ? Correct AddDbContext syntax:
builder.Services.AddDbContext<EmployeeManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandelingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

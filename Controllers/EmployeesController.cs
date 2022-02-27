using Certigon.Test.Dto;
using Certigon.Test.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Certigon.Test.Controllers;

public class EmployeesController : ControllerBase
{
    private readonly EmployeeContainer employeeContainer;

    public EmployeesController(EmployeeContainer employeeStore)
    {
        this.employeeContainer = employeeStore;
    }
    
    [HttpGet("api/employees")]
    public List<Employee> GetAll()
    {
        return employeeContainer.Employees.ToList();
    }

    [HttpGet("api/employees/{isActive}")]
    public List<Employee> Get(bool isActive)
    {
        return employeeContainer.Employees.Where(x => x.IsActive == isActive).ToList();
    }

    [HttpGet("api/employee/{id}")]
    public ActionResult<Employee> GetById(int id)
    {
        var result = employeeContainer.Employees.FirstOrDefault(x => x.Id == id);
        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpGet("api/department/{departmentId}/employees/{isActive?}")]
    public List<Employee> GetByDepartment(int departmentId, bool? isActive = true)
    {
        if (!Enum.IsDefined(typeof(Department), departmentId))
        {
            throw new ArgumentOutOfRangeException($"{departmentId} is not an underlying value of Department enumeration.");
        }

        return employeeContainer.Employees.Where(x => x.DepartmentId == (Department)departmentId && x.IsActive == isActive).ToList();
    }

    [HttpPost("api/employees")]
    public Employee Create([FromBody] CreateEmployeeDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        return employeeContainer.Insert(input);
    }

    [HttpPut("api/employees")]
    public Task Update([FromBody] UpdateEmployeeDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        employeeContainer.Update(input);

        return Task.CompletedTask;
    }

    [HttpDelete("api/employees/{id}")]
    public Task Delete(int id)
    {
        employeeContainer.Delete(id);

        return Task.CompletedTask;
    }
}
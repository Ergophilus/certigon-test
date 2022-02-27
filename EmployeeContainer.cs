using Certigon.Test.Dto;
using Certigon.Test.Entities;

namespace Certigon.Test;

public class EmployeeContainer
{
    private readonly List<Employee> initial = new()
    {
        new Employee
        {
            Id = 1,
            Name = "Janko",
            Surname = "Janković",
            IsActive = true,
            DepartmentId = Department.Development
        },
        new Employee
        {
            Id = 2,
            Name = "Marko",
            Surname = "Marković",
            IsActive = true,
            DepartmentId = Department.Management
        },
        new Employee
        {
            Id = 3,
            Name = "Petar",
            Surname = "Perović",
            IsActive = true,
            DepartmentId = Department.HR
        },
        new Employee
        {
            Id = 4,
            Name = "Gruna",
            Surname = "Stramen",
            IsActive = false,
            DepartmentId = Department.Development
        },
        new Employee
        {
            Id = 5,
            Name = "Milorad",
            Surname = "Krušica",
            IsActive = false,
            DepartmentId = Department.Management
        },
        new Employee
        {
            Id = 6,
            Name = "Durio",
            Surname = "Uturinović",
            IsActive = false,
            DepartmentId = Department.HR
        },
    };

    private readonly List<Employee> employees;

    public EmployeeContainer()
    {
        employees ??= initial;
    }

    public IQueryable<Employee> Employees => employees.AsQueryable();

    public Employee Insert(CreateEmployeeDto input)
    {
        var employee = new Employee
        {
            Id = employees.Max(x => x.Id) + 1,
            Name = input.Name,
            Surname = input.Surname,
            IsActive = input.IsActive,
            DepartmentId = input.DepartmentId
        };

        employees.Add(employee);

        return employee;
    }

    public void Update(UpdateEmployeeDto input)
    {
        var employee = employees.FirstOrDefault(x => x.Id == input.Id);
        if (employee == null)
        {
            throw new Exception("Employee not found!");
        }

        employee.Name = input.Name;
        employee.Surname = input.Surname;
        employee.IsActive = input.IsActive;
        employee.DepartmentId = input.DepartmentId;

        var indexOfEmployee = employees.IndexOf(employee);
        employees[indexOfEmployee] = employee;
    }

    public void Delete(int id)
    {
        var employee = employees.FirstOrDefault(x => x.Id == id);
        if (employee == null)
        {
            throw new Exception("Employee not found!");
        }

        employees.Remove(employee);
    }
}
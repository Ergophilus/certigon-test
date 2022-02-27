namespace Certigon.Test.Entities;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public bool IsActive { get; set; }

    public Department DepartmentId { get; set; }
}
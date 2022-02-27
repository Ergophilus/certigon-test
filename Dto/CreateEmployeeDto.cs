using System.ComponentModel.DataAnnotations;
using Certigon.Test.Entities;

namespace Certigon.Test.Dto;

public class CreateEmployeeDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    [StringLength(128)]
    public string Surname { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public Department DepartmentId { get; set; }
}
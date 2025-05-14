using System.ComponentModel.DataAnnotations;
using SampleApi.Entity;

namespace SampleApi.Dto;

public class EmployeeSaveUpdate
{
    public long? Id { get; set; }
    [Required]
    [MaxLength(50, ErrorMessage = "data tidak boleh lebih dari 50 kata!!")]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public long Salary { get; set; }
    
    public string Address { get; set; }
    
    public long DivisionId { get; set; }
    
   
}
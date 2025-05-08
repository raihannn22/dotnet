using System.ComponentModel.DataAnnotations;

namespace SampleApi.Dto;

public class EmployeeRequest
{
    [Required]
    [MaxLength(50, ErrorMessage = "data tidak boleh lebih dari 50 kata!!")] 
    public string Name { get; set; }
    public string Email { get; set; }
    public long Salary { get; set; }
    public string Address { get; set; }
    public long DivisionId { get; set; }
    
    
}
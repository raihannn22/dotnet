using System.ComponentModel.DataAnnotations;

namespace SampleApi.Dto;

public class EmployeeSaveUpdate
{
    public long? Id { get; set; }
    [Required]
    [MaxLength(50, ErrorMessage = "data tidak boleh lebih dari 50 kata!!")]
    public string Name { get; set; }
}
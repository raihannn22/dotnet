using System.ComponentModel.DataAnnotations;

namespace SampleApi.Dto;

public class EmployeeSaveUpdate
{
    public long? Id { get; set; }
    [Required]
    [MaxLength(10, ErrorMessage = "data tidak boleh lebih dari 10 kata!!")]
    public string Name { get; set; }
}
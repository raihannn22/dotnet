using System.ComponentModel.DataAnnotations;

namespace SampleApi.Dto;

public class LoginRequest
{
    [Required]
    [MaxLength(30, ErrorMessage = "data tidak boleh lebih dari 30 kata!!")]
    [MinLength(5, ErrorMessage = "data tidak boleh kurang dari 5 kata!!")]
    public string Username { get; set; }
    
    [Required]
    [MaxLength(30, ErrorMessage = "data tidak boleh lebih dari 30 kata!!")]
    [MinLength(5, ErrorMessage = "data tidak boleh kurang dari 5 kata!!")]
    public string Password { get; set; }
}
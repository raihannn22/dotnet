namespace SampleApi.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class AppUser
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    // public string Role { get; set; } // Optional, untuk role-based auth
}
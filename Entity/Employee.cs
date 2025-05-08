using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApi.Entity;


[Table("raihan_employee")]
public class Employee : Auditable
{
    [Key]
    public long Id { get; set; }
    [Column]
    public string Name { get; set; }
    [Column]
    public bool IsDeleted { get; set; } = false;
}
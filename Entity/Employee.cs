using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApi.Entity;


[Table("employee")]
public class Employee
{
    [Key]
    public long Id { get; set; }
    [Column]
    public string Name { get; set; }
}
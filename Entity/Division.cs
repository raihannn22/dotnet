using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApi.Entity;

[Table("raihan_division")]
public class Division : Auditable
{
    [Key]
    public long Id { get; set; }
    [Column ("name")]
    public string Name { get; set; }
    
    public ICollection<Employee> Employees { get; set; }
}
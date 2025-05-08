using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApi.Entity;


[Table("raihan_employee")]
public class Employee : Auditable
{
    [Key]
    public long Id { get; set; }
    [Column ("name")]
    public string Name { get; set; }
    [Column("deleted")]
    public bool IsDeleted { get; set; } = false;
    [Column ("email")]
    public string Email { get; set; }
    [Column("salary")]
    public long Salary { get; set; }
    [Column ("address")]
    public string Address { get; set; }
    
    public long DivisionId { get; set; }
    public Division Division { get; set; }
}
using Microsoft.EntityFrameworkCore;
using SampleApi.Entity;

namespace SampleApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){} 
    
    public DbSet<Employee> Employees { get; set; } 
    
}
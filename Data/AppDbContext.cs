using Microsoft.EntityFrameworkCore;
using SampleApi.Entity;

namespace SampleApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){} 
    
    public DbSet<Employee> Employees { get; set; } 
    
    public DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "$2a$11$ZTtHwldYZrKfR7hDU3kL2OhfMZ43m/OZNZB4x8z3VNTqpEvVleIlm", // Hash dari "1234"
            }
        );
    }
}
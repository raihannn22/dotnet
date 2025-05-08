using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SampleApi.Entity;

namespace SampleApi.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    } 
    
    public DbSet<Employee> Employees { get; set; } 
    
    public DbSet<AppUser> Users { get; set; }
    
    public DbSet<Division> Divisions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "$2a$12$0jXbBkQmLp0r.sOmacTLWuvE2N9Fj3wRXa6wOlyV1vdXkbw5tOYoa", // Hash dari "123456"
            }
        );

        modelBuilder.Entity<Division>().HasData(
            new Division
            {
                Id = 1,
                Name = "Information Technology",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc),
                CreatedBy = "admin"
            },
            
            new Division
            {
                Id = 2,
                Name = "Human Reasource",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc),
                CreatedBy = "admin"
            }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var CurrentUser = getCurrentUsername();
        var CurrentTime = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<Auditable>() )
        {
            switch (entry.State)
            {
                case EntityState.Added :
                    entry.Entity.CreatedAt = CurrentTime;
                    entry.Entity.CreatedBy = CurrentUser;
                    entry.Entity.UpdatedAt = null;
                    entry.Entity.UpdatedBy = null;
                    break;
                case EntityState.Modified :
                    entry.Entity.UpdatedAt = CurrentTime;
                    entry.Entity.UpdatedBy = CurrentUser;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public string getCurrentUsername()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return "Anonymous";
        }
        return httpContext.User.Identity.Name ?? httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unknown";
    }
}
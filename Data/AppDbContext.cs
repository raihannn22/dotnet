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
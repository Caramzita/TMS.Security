using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TMS.Security.Core;

namespace TMS.Security.DataAccess;

public class DataBaseContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;

    //public DbSet<User> Users {  get; set; }

    public DataBaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>(entity => entity.ToTable(name: "Users"));
        modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));

        modelBuilder.Entity<IdentityUserRole<string>>(entity => 
            entity.ToTable(name: "UserRoles"));

        modelBuilder.Entity<IdentityUserClaim<string>>(entity => 
            entity.ToTable(name: "UserClaims"));

        modelBuilder.Entity<IdentityUserLogin<string>>(entity => 
            entity.ToTable(name: "UserLogins"));

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            entity.ToTable(name: "UserTokens"));

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            entity.ToTable(name: "RoleClaims"));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection"))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
    }
}

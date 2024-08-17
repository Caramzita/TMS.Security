using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess;

public class DataBaseContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<UserDto> Users { get; set; }

    public DbSet<RefreshTokenDto> RefreshTokens { get; set; }

    public DataBaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection"))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess;

/// <summary>
/// Контекст базы данных для работы с пользовательскими данными и токенами обновления.
/// </summary>
public class DataBaseContext : DbContext
{
    /// <summary>
    /// Конфигурация сервиса.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<DataBaseContext> _logger;

    /// <summary>
    /// Таблица пользователей.
    /// </summary>
    public DbSet<UserDto> Users { get; set; }

    /// <summary>
    /// Таблица токенов обновления.
    /// </summary>
    public DbSet<RefreshTokenDto> RefreshTokens { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DataBaseContext"/>.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="logger">Логгер для записи информации о действиях в контексте базы данных.</param>
    public DataBaseContext(IConfiguration configuration, ILogger<DataBaseContext> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Настраивает сущности модели с использованием конфигураций из сборки.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели, используемый для настройки сущностей.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Настраивает параметры подключения к базе данных.
    /// </summary>
    /// <param name="optionsBuilder">Построитель параметров, используемый для настройки подключения.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection"))
            .EnableSensitiveDataLogging()
            .LogTo(log => _logger.LogInformation(log));
    }
}

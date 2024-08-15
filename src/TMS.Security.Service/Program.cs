using Microsoft.AspNetCore.Identity;
using TMS.Security.DataAccess;
using TMS.Security.UseCases.Commands.Registration;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Services;
using TMS.Security.Integration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
builder.Services.AddSwagger(xmlFilePath);

builder.Services.AddDbContext<DataBaseContext>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

//builder.Configuration
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<IdentityOptions>(options =>
{
    // Настройка опций идентификации
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;

    // Настройки блокировки
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
//    db.Database.Migrate();
//}

app.Run();

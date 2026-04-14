using API.Contracts;
using API.Data;
using API.Exceptions;
using API.Extensions;
using API.Middlewares;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Bootstrap Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                                   .AddJsonFile("appsettings.json")
                                   .AddJsonFile(
                                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                                    ?? "Production"}.json",
                                        true)
                                   .Build())
            .CreateLogger();

try
{
    Log.Information("Starting TaskManagementAPI...");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddSerilog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    // Db Context
    builder.Services.AddDbContext<AppDbContext>(options =>
                                                    options.UseSqlServer(
                                                        builder.Configuration.GetConnectionString("DefaultConnection")));

    // Register Repositories
    builder.Services.AddScoped<ITaskRepository, TaskRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    // Middlewares
    builder.Services.AddScoped<RequestLoggingMiddleware>();
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    // Lifetime demonstration services
    builder.Services.AddSingleton<SingletonDemoService>();
    builder.Services.AddScoped<ScopedDemoService>();
    builder.Services.AddTransient<TransientDemoService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandling();
    
    app.UseRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "HostAbortedException" and not "StopTheHostException")
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
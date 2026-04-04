using RescueSystem.Application;
using RescueSystem.Infrastructure;
using RescueSystem.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Apply migrations and create database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rescue System API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

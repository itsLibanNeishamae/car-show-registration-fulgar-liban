using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Api.Endpoints.Registrations;
using Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=carshow.db";
var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(rawConnectionString);
if (!Path.IsPathRooted(connectionStringBuilder.DataSource))
{
    var parentPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", connectionStringBuilder.DataSource));
    var localPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, connectionStringBuilder.DataSource));
    if (File.Exists(parentPath))
    {
        connectionStringBuilder.DataSource = parentPath;
    }
    else if (File.Exists(localPath))
    {
        connectionStringBuilder.DataSource = localPath;
    }
    else
    {
        connectionStringBuilder.DataSource = parentPath;
    }
}
var connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDevelopment", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
    options.SerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip;
});

var app = builder.Build();
app.UseCors("ReactDevelopment");

// Migrate applies pending migrations and preserves existing data across restarts.
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await RegistrationsSeeder.SeedAsync(dbContext);
}

app.MapGet("/", () => Results.Ok(new { service = "Car Show Registration API" }));
app.MapRegistrationEndpoints();
app.Run();

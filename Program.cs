using Microsoft.EntityFrameworkCore;
using Npgsql;
using CareLink_Refugee.Persistence;
using CareLink_Refugee.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "cl";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
var host = Environment.GetEnvironmentVariable("HOST") ?? "http://localhost";
var port = Environment.GetEnvironmentVariable("PORT") ?? "5169";
var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var pgDataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
var dataSource = pgDataSourceBuilder.Build();
builder.Services.AddDbContext<RefugeeDbContext>(options =>
    options.UseNpgsql(dataSource));

builder.WebHost.UseUrls($"{host}:{port}");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<RefugeeDbContext>();

    dbContext.Database.Migrate();
    DbInitializer.Initialize(dbContext);
}

app.UseAuthorization();
app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
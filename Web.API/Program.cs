using Application;
using Infrastructure;
using Serilog;
using Web.API;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);
builder.Services.AddInfrastructure();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

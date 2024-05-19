using Application;
using Contracts;
using Web.API;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddContracts();
builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

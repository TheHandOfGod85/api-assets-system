using Application;
using Infrastructure;
using Serilog;
using Web.API;
using sib_api_v3_sdk.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddDatabase(configuration);
builder.Services.AddInfrastructure();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//email configuration
var mailSettings = new MailSettings();
configuration.Bind(nameof(MailSettings), mailSettings);
Configuration.Default.ApiKey.Add("api-key", configuration["MailSettings:Key"]);
var mailSection = configuration.GetSection(nameof(MailSettings));
builder.Services.Configure<MailSettings>(mailSection);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

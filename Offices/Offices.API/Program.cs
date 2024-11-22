using Offices.API.Extensions;
using Offices.API.Infrastructure;
using Offices.Application.Extensions;
using Offices.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Adding problem details
builder.Services.AddProblemDetails();

// Configuring container
builder.Services.AddServices();
builder.Services.AddApiExtensions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.MapControllers();

app.Run();
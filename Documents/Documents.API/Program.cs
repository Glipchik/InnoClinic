using Documents.API.DependencyInjection;
using Documents.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddUserSecrets<Program>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApiDependencyInjection(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.MapControllers();

app.Run();

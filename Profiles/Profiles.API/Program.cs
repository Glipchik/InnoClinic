using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Profiles.API.DependencyInjection;
using Profiles.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddUserSecrets<Program>();

builder.Services.AddApiDependencyInjection(configuration);

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.UseCors("AllowLocalhost3000");
app.UseAuthentication();
app.UseAuthorization();

app.Run();

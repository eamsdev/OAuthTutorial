using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using OAuthDemo.Application;
using OAuthDemo.Domain;
using OAuthDemo.Infrastructure;
using OAuthDemo.Infrastructure.Persistence;
using OAuthDemo.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddWebServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddDomainServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsRelational())
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.UseCors("CorsPolicy");
app.UseProblemDetails();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.Run();

public partial class Program { }
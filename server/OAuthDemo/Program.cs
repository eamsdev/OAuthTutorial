using Microsoft.AspNetCore.Authentication;
using OAuthDemo.Application;
using OAuthDemo.Domain;
using OAuthDemo.Infrastructure;
using OAuthDemo.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddWebServices(builder.Configuration)
    .AddInfrastructureServices()
    .AddApplicationServices()
    .AddDomainServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
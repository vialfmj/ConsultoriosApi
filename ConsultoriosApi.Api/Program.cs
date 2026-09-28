using ConsultoriosApi.Api.Middleware;
using ConsultoriosApi.Application;
using ConsultoriosApi.Infrastructure;
using ConsultoriosApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionsHandlerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

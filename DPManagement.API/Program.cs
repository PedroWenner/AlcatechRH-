using DPManagement.API.Extensions;
using DPManagement.Application;
using DPManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Services from Layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Extension Services (Auth, CORS, Swagger)
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddApiCors();
builder.Services.AddApiDocumentation();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiDocumentation(app.Environment);

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

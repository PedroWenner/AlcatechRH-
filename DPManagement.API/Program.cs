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
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<DPManagement.Application.Interfaces.IUserContext, DPManagement.API.Services.UserContextService>();


var app = builder.Build();

// Run Seeds
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DPManagement.Infrastructure.Persistence.DPManagementDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var csvPath = Path.Combine(env.ContentRootPath, "Data", "Seed", "cbo2002.csv");
    await DPManagement.API.Data.Seed.CboIngestion.SeedCbosAsync(context, csvPath);
}

// Configure the HTTP request pipeline.
app.UseApiDocumentation(app.Environment);

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

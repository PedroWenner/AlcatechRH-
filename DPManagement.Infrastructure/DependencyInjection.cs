using DPManagement.Domain.Repositories;
using DPManagement.Infrastructure.Persistence;
using DPManagement.Infrastructure.Repositories;
using DPManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DPManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DPManagementDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
        services.AddScoped<ICargoRepository, CargoRepository>();
        services.AddScoped<ICboRepository, CboRepository>();
        services.AddHttpClient<IViaCepService, ViaCepService>();
        services.AddScoped<DPManagement.Application.Interfaces.IPerfilService, Services.PerfilService>();
        
        return services;
    }
}

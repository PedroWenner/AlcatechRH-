using DPManagement.Infrastructure.Persistence;
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

        // Repositórios, UnitOfWork e outros serviços de infraestrutura podem ser injetados aqui.
        services.AddScoped<DPManagement.Application.Interfaces.IPerfilService, Services.PerfilService>();
        
        return services;
    }
}

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
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddHttpClient<IViaCepService, ViaCepService>();
        services.AddScoped<DPManagement.Application.Interfaces.IPerfilService, Services.PerfilService>();
        services.AddScoped<DPManagement.Application.Interfaces.IPermissaoService, Services.PermissaoService>();
        services.AddScoped<DPManagement.Application.Interfaces.IDadoBancarioService, Services.DadoBancarioService>();
        services.AddScoped<DPManagement.Application.Interfaces.IBancoService, Services.BancoService>();
        services.AddScoped<DPManagement.Application.Interfaces.IOrgaoService, Services.OrgaoService>();
        services.AddScoped<DPManagement.Application.Interfaces.ICentroCustoService, Services.CentroCustoService>();
        
        return services;
    }
}

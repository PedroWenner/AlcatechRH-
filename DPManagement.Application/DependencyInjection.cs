using DPManagement.Application.Interfaces;
using DPManagement.Application.Services;
using DPManagement.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DPManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICargoAppService, CargoAppService>();
        services.AddScoped<IColaboradorAppService, ColaboradorAppService>();
        services.AddScoped<ICboAppService, CboAppService>();
        
        services.AddValidatorsFromAssemblyContaining<CreateColaboradorValidator>();

        return services;
    }
}

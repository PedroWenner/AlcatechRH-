using DPManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DPManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        // Outros serviços de aplicação de diferentes módulos podem ser injetados aqui. 
        // Em um cenário altamente modular, poderíamos usar Reflection para carregar módulos dinamicamente.
        
        return services;
    }
}

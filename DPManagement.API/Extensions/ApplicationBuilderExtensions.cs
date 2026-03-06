using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace DPManagement.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            var webApp = app as WebApplication;
            webApp?.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "DP Management API");
            });
        }

        return app;
    }
}

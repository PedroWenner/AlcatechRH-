using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace DPManagement.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "DP Management API v1");
                options.RoutePrefix = "swagger";
            });
        }

        return app;
    }
}

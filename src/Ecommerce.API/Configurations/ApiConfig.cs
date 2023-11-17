using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection  AddWebApiConfig(this IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development", builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseWebApiConfiguration(this IApplicationBuilder app)
        {
            app.UseCors("Development");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}

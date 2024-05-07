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

                options.AddPolicy("Production", builder =>
                {
                    builder
                        .WithMethods("GET")
                        .WithOrigins("https://localhost")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseWebApiConfiguration(this IApplicationBuilder app , IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}

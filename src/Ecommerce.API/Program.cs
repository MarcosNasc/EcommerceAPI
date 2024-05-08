
using Ecommerce.API.Configurations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Ecommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddContextsConfig(configuration);
            builder.Services.AddIdentityConfig(configuration);
            builder.Services.ResolveDependencies();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddWebApiConfig();
            builder.Services.AddSwaggerConfig();
            builder.Services.AddEndpointsApiExplorer();
         
            var app = builder.Build();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            
            app.UseWebApiConfiguration(app.Environment);

            app.UseSwaggerConfig(provider);
            
            app.MapControllers();

            app.Run();
        }
    }
}
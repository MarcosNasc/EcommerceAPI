
using Ecommerce.API.Configurations;

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            app.UseWebApiConfiguration(app.Environment);

            app.MapControllers();

            app.Run();
        }
    }
}
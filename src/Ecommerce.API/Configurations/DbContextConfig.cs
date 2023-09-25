using Ecommerce.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddContextsConfig(this IServiceCollection services , IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<EcommerceDBContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}

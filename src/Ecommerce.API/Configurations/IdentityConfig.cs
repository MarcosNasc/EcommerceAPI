using Ecommerce.API.Data;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.API.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            return services;
        }
    }
}

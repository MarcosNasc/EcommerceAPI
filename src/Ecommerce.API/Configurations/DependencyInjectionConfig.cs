using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Ecommerce.BLL.Services;
using Ecommerce.DAL.Repository;

namespace Ecommerce.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<ISupplierService, SupplierService>();
            #endregion

            #region Repositories
            services.AddScoped<ISupplierRepository,SupplierRepository>();
            #endregion

            return services;
        }
    }
}

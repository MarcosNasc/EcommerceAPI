using Ecommerce.API.Extensions;
using Ecommerce.BLL.Interfaces;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Ecommerce.BLL.Notifications;
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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<INotificator, Notificator>();

            #endregion

            #region Repositories
            services.AddScoped<ISupplierRepository,SupplierRepository>();
            services.AddScoped<IAddressRepository,AddressRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            #endregion

            #region Extensions
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, User>();
            #endregion

            return services;
        }
    }
}

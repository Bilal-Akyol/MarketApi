using FluentValidation.AspNetCore;
using MarketBusiness.Abstract;
using MarketBusiness.Concrete;
using MarketData.Abstract;
using MarketData.Concrete.Ef;
using MarketEntity.Models;
using Microsoft.AspNetCore.Identity;


namespace MarketApi.Extensions
{
    public static class DependencyInjectionServiceExtensions
    {
        public static IServiceCollection AddDependency(this IServiceCollection services) 
        {
            services.AddControllersWithViews()
                    .AddFluentValidation();

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IListService, ListService>();



            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<ICategoriesRepository, EfCategoriesRepository>();
            services.AddScoped<IProductRepository, EfProductRepository>();
            services.AddScoped<IProductImageRepository, EfProductImageRepository>();
            services.AddScoped<ISliderRepository, EfSliderRepository>();
            services.AddScoped<IAboutRepository, EfAboutRepository>();
            services.AddScoped<IContactRepository, EfContactRepository>();
            services.AddScoped<ILogoRepository, EfLogoRepository>();




            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
                
        }
    }
}

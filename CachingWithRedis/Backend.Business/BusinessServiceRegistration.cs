using Backend.Business.CacheService.Concretes;
using Backend.Business.CacheService.Interfaces;
using Backend.Business.Services.Concretes;
using Backend.Business.Services.Interfaces;
using Backend.Data.Repositories.Concretes;
using Backend.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Business;

public static class BusinessServiceRegistration
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
    }
}
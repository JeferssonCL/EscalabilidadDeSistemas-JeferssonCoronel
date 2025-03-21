using Backend.Business.BackgroundServices;
using Backend.Data.Models;
using Backend.Data.Repositories.Read.Concretes;
using Backend.Data.Repositories.Read.Interfaces;
using Backend.Data.Repositories.Write.Concretes;
using Backend.Data.Repositories.Write.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Business;

public static class ApplicationConfiguration
{
    public static void AddConfigurations(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(ApplicationConfiguration).Assembly));

        services.AddScoped<IWriteRepository<Product>, ProductWriteRepository>();
        services.AddScoped<IReadRepository<ProductReadModel>, ProductReadRepository>();
        
        services.AddHostedService<DataSyncBackgroundService>();
    }
}
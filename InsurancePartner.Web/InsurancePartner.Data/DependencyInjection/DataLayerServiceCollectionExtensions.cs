namespace InsurancePartner.Data.DependencyInjection;

using Configurations;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

public static class DataLayerServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConfig = new DatabaseConfig
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection")
        };

        services.AddSingleton(dbConfig);

        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();

        return services;
    }
}

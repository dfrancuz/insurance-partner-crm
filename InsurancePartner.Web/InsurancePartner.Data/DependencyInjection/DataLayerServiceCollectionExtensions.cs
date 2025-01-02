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
        var connectionString = configuration.GetSection("DatabaseConfig:ConnectionString").Value;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException("The ConnectionString property has not been initialized or is missing.");
        }
        var dbConfig = new DatabaseConfig
        {
            ConnectionString = connectionString
        };

        services.AddSingleton(dbConfig);

        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();

        return services;
    }
}

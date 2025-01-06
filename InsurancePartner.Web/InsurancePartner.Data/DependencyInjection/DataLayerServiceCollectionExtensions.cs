using Microsoft.Data.SqlClient;

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

        TestDatabaseConnection(connectionString);

        var dbConfig = new DatabaseConfig
        {
            ConnectionString = connectionString
        };

        services.AddSingleton(dbConfig);

        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();

        return services;
    }

    private static void TestDatabaseConnection(string connectionString)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1";
                    command.ExecuteScalar();
                }
            }
        }
        catch (SqlException e)
        {
            throw new ApplicationException(
                "Failed to establish database connection during application startup. " +
                "Please check your connection string and ensure the database is accessible.",
                e);
        }
        catch (Exception e)
        {
            throw new ApplicationException(
                "An unexpected error occurred while testing database connection during startup.", e);
        }
    }
}

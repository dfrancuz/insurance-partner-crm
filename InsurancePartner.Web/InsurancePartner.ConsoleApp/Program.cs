namespace InsurancePartner.ConsoleApp;

using Data.Configurations;
using Microsoft.Extensions.Configuration;

public static class Program
{
    public static void Main()
    {
        Test();
    }

    private static void Test()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var dbConfig = new DatabaseConfig()
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
        };

        try
        {
            Console.WriteLine("ConsoleApp will be used for testing before MVC application is live...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

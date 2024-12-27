using InsurancePartner.Data.Configurations;
using InsurancePartner.Data.Models;
using InsurancePartner.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace InsurancePartner.ConsoleApp;

public static class Program
{
    public static void Main()
    {
        Test().Wait();
    }

    private static async Task Test()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var dbConfig = new DatabaseConfig()
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
        };

        var partnerRepo = new PartnerRepository(dbConfig);
        var policyRepo = new PolicyRepository(dbConfig);

        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            PartnerTypeId = 1,
            CreateByUser = "test@example.com",
            IsForeign = false,
            ExternalCode = "TESTCODE12345",
            Gender = 'M'
        };

        try
        {
            Console.WriteLine("Creating partner...");
            var id = await partnerRepo.CreatePartnerAsync(partner);
            Console.WriteLine($"Created partner with ID: {id}");

            Console.WriteLine("\nFetching all partners:");
            var partners = await partnerRepo.GetAllPartnersAsync();
            foreach (var p in partners)
            {
                Console.WriteLine($"- {p.FirstName} {p.LastName} (ID: {p.PartnerId})");
            }

            var policy = new Policy
            {
                PartnerId = id,
                PolicyNumber = "POL1234567890",
                Amount = 1000.00m
            };

            Console.WriteLine("\nCreating policy...");
            var policyId = await policyRepo.CreatePolicyAsync(policy);
            Console.WriteLine($"Created policy with ID: {policyId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

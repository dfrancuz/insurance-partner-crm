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
            FirstName = "Aaron",
            LastName = "King",
            PartnerNumber = "12345670000234567890",
            PartnerTypeId = 1,
            CreateByUser = "aaron@example.com",
            IsForeign = false,
            ExternalCode = "TESTCODE99999",
            Gender = 'M'
        };

        var partner1 = new Partner
        {
            FirstName = "Jessica",
            LastName = "Jefferson",
            PartnerNumber = "12345623201234567894",
            PartnerTypeId = 1,
            CreateByUser = "test2@example.com",
            IsForeign = false,
            ExternalCode = "TESTAAAA12345",
            Gender = 'F'
        };

        try
        {
            Console.WriteLine("Creating partner...");
            var id = await partnerRepo.CreatePartnerAsync(partner);
            Console.WriteLine($"Created partner with ID: {id}");

            var id2 =  await partnerRepo.CreatePartnerAsync(partner1);
            Console.WriteLine($"Created partner with ID: {id2}");

            Console.WriteLine("\nFetching all partners:");
            var partners = await partnerRepo.GetAllPartnersAsync();
            foreach (var p in partners)
            {
                Console.WriteLine($"- {p.FirstName} {p.LastName} (ID: {p.PartnerId})");
            }

            var policy = new Policy
            {
                PartnerId = id,
                PolicyNumber = "POL1664567899",
                Amount = 109000.00m
            };

            Console.WriteLine("\nCreating policy...");
            var policyId = await policyRepo.CreatePolicyAsync(policy);
            Console.WriteLine($"Created policy with ID: {policyId}");

            Console.WriteLine("\nFetching partner with policies:");
            var partnerWithPolicies = await partnerRepo.GetPartnerByIdAsync(id);
            Console.WriteLine($"Partner: {partnerWithPolicies.FirstName} {partnerWithPolicies.LastName}");
            Console.WriteLine($"Number of policies: {partnerWithPolicies.Policies.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

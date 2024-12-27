using InsurancePartner.Data.Configurations;
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

        try
        {
            Console.WriteLine("\nChecking if partner number exists...");
            var partnerNumberExists = await partnerRepo.PartnerNumberExistsAsync("12345678901234567890");
            Console.WriteLine($"Partner number '{"12345678901234567890"}' exists: {partnerNumberExists}");

            Console.WriteLine("\nFetching partner types...");
            var partnerTypes = await partnerRepo.GetPartnerTypesAsync();
            Console.WriteLine($"Partner Types: {string.Join(", ", partnerTypes.Select(pt => pt.TypeName))}");

            Console.WriteLine("\nChecking if policy number exists...");
            var policyNumberExists = await policyRepo.PolicyNumberExistsAsync("POL1664567899");
            Console.WriteLine($"Policy number '{"POL1664567899"}' exists: {policyNumberExists}");

            Console.WriteLine("\nChecking if policy number exists...");
            var policyNumberExists2 = await policyRepo.PolicyNumberExistsAsync("POL0000567899");
            Console.WriteLine($"Policy number '{"POL1664567899"}' exists: {policyNumberExists2}");

            Console.WriteLine("\nFetching policies for partner...");
            var partnerPolicies = await policyRepo.GetPartnerPoliciesAsync(1);
            Console.WriteLine($"Policies for Partner {1}: {string.Join(", ", partnerPolicies.Select(p => p.PolicyNumber))}");

            Console.WriteLine("\nFetching policies for partner...");
            var partnerPolicies2 = await policyRepo.GetPartnerPoliciesAsync(3);
            Console.WriteLine($"Policies for Partner {3}: {string.Join(", ", partnerPolicies2.Select(p => p.PolicyNumber))}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

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

        try
        {
            Console.WriteLine("\nUpdating partner...");
            var partnerToUpdate = new Partner
            {
                PartnerId = 1,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Address = "UpdatedAddress",
                PartnerNumber = "12345678901234567890",
                CroatianPIN = "12345678911",
                PartnerTypeId = 2,
                CreateByUser = "test@example.com",
                IsForeign = false,
                ExternalCode = "TESTCODE12345",
                Gender = 'M'
            };

            var updateResult = await partnerRepo.UpdatePartnerAsync(partnerToUpdate);
            Console.WriteLine(updateResult ? "Update successful" : "Update failed");

            Console.WriteLine("\nDeleting partner...");
            var partnerIdToDelete = 1;
            var deleteResult = await partnerRepo.DeletePartnerAsync(partnerIdToDelete);
            Console.WriteLine(deleteResult ? "Delete successful" : "Delete failed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

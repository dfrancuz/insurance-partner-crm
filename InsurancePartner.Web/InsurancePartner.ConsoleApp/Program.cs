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
            Console.WriteLine("\nCreating a new partner...");
            var newPartner = new Partner
            {
                FirstName = "Some name",
                LastName = "Some last name",
                Address = "1234 Elm Street",
                PartnerNumber = "11111111111111100000",
                CroatianPIN = "12345600000",
                PartnerTypeId = 1,
                CreateByUser = "test20@example.com",
                IsForeign = false,
                ExternalCode = "EXT0987777",
                Gender = 'M'
            };

            var createdPartnerId = await partnerRepo.CreatePartnerAsync(newPartner);
            Console.WriteLine($"New partner created with ID: {createdPartnerId}");

            Console.WriteLine("\nFetching all partners...");
            var partners = await partnerRepo.GetAllPartnersAsync();
            foreach (var partner in partners)
            {
                Console.WriteLine($"Name: {partner.FirstName} {partner.LastName}");
            }

            Console.WriteLine("\nUpdating the newly created partner...");
            newPartner.PartnerId = createdPartnerId;
            newPartner.FirstName = "UpdatedFirstName";
            var updateResult = await partnerRepo.UpdatePartnerAsync(newPartner);
            Console.WriteLine(updateResult ? "Update successful" : "Update failed");

            Console.WriteLine("\nFetching partner by ID...");
            var fetchedPartner = await partnerRepo.GetPartnerByIdAsync(createdPartnerId);
            if (fetchedPartner != null)
            {
                Console.WriteLine(
                    $"Fetched Partner ID: {fetchedPartner.PartnerId}, Name: {fetchedPartner.FirstName} {fetchedPartner.LastName}");
            }
            else
            {
                Console.WriteLine("Partner not found.");
            }

            Console.WriteLine("\nDeleting the partner...");
            var deleteResult = await partnerRepo.DeletePartnerAsync(createdPartnerId);
            Console.WriteLine(deleteResult ? "Delete successful" : "Delete failed");

            Console.WriteLine("\nCreating a new policy...");
            var newPolicy = new Policy
            {
                PolicyNumber = "POL123456400",
                Amount = 1900.99m
            };

            var createdPolicyId = await policyRepo.CreatePolicyAsync(newPolicy);
            Console.WriteLine($"New policy created with ID: {createdPolicyId}");

            Console.WriteLine("\nAssigning policy to partner...");
            var assignResult = await policyRepo.AssignPolicyToPartnerAsync(createdPolicyId, 3);
            Console.WriteLine(assignResult > 0
                ? "Policy assigned to partner successfully"
                : "Policy assignment failed");

            Console.WriteLine("\nFetching partner's policies...");
            var policies = await policyRepo.GetPartnerPoliciesAsync(3);
            foreach (var policy in policies)
            {
                Console.WriteLine(
                    $"Policy ID: {policy.PolicyId}, Policy Number: {policy.PolicyNumber}, Amount: {policy.Amount}");
            }

            Console.WriteLine("\nRemoving policy from partner...");
            var removeResult = await policyRepo.RemovePolicyFromPartnerAsync(createdPolicyId, 3);
            Console.WriteLine(removeResult ? "Policy removed from partner successfully" : "Policy removal failed");

            Console.WriteLine("\nChecking if external code exists...");
            var externalCodeExists = await partnerRepo.ExternalCodeExistsAsync("EXTCODE123");
            Console.WriteLine(externalCodeExists ? "External code exists" : "External code does not exist");

            Console.WriteLine("\nChecking if partner number exists...");
            var partnerNumberExists = await partnerRepo.PartnerNumberExistsAsync("987654321");
            Console.WriteLine(partnerNumberExists ? "Partner number exists" : "Partner number does not exist");

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

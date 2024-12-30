using InsurancePartner.Data.Configurations;
using InsurancePartner.Data.Repositories;
using InsurancePartner.Logic.DTOs;
using InsurancePartner.Logic.Services;
using InsurancePartner.Logic.Validators;
using Microsoft.Extensions.Configuration;

namespace InsurancePartner.ConsoleApp;

public static class Program
{
    public static async Task Main()
    {
        await TestAsync();
    }

    private static async Task TestAsync()
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbConfig = new DatabaseConfig
            {
                ConnectionString = configuration.GetConnectionString("DefaultConnection")
            };

            var partnerRepo = new PartnerRepository(dbConfig);
            var policyRepo = new PolicyRepository(dbConfig);

            var createPartnerValidator = new CreatePartnerValidator(partnerRepo);
            var updatePartnerValidator = new UpdatePartnerValidator(partnerRepo);

            var partnerService =
                new PartnerService(partnerRepo, policyRepo, createPartnerValidator, updatePartnerValidator);

            var newPartner = new CreatePartnerDto
            {
                FirstName = "Partner1 FirstName",
                LastName = "Partner1 LastName",
                PartnerNumber = "99999999991111111111",
                PartnerTypeId = 1,
                CreateByUser = "user1@user1.com",
                IsForeign = true,
                ExternalCode = "111222333444",
                Gender = 'M',
                SelectedPolicyIds = [1, 3]
            };

            // 1. Create a new partner...
            var createdResult = await partnerService.CreatePartnerAsync(newPartner);
            Console.WriteLine(createdResult.IsSuccess
                ? "Partner created successfully"
                : $"Failed to create partner: {createdResult.Message}");

            // 2. Get all partners...
            var partners = await partnerService.GetAllPartnersAsync();
            foreach (var partner in partners)
            {
                Console.WriteLine($"Partner: {partner.FirstName} {partner.LastName}, Partner Number: {partner.PartnerNumber}");
            }

            // 3. Update a partner...
            var existingPartner = partners.FirstOrDefault();
            if (existingPartner != null)
            {
                existingPartner.FirstName = "Updated Partner Name";
                existingPartner.Address = "465 Updated Street";
                existingPartner.PartnerNumber = "99999999991111111111";
                existingPartner.ExternalCode = "111222333444";

                var updateResult = await partnerService.UpdatePartnerAsync(existingPartner);
                Console.WriteLine(updateResult.IsSuccess
                    ? "Partner updated successfully"
                    : $"Failed to update partner: {updateResult.Message}");
            }

            // 4. Delete a partner...
            if (existingPartner != null)
            {
                var deleteResult = await partnerService.DeletePartnerAsync(existingPartner.PartnerId);
                Console.WriteLine(deleteResult.IsSuccess
                    ? "Partner deleted successfully"
                    : $"Failed to delete partner: {deleteResult.Message}");
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

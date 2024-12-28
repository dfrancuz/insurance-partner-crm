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

        var policyRepo = new PolicyRepository(dbConfig);

        try
        {
            Console.WriteLine("Testing Policy Repository Methods...");

            var newPolicy = new Policy
            {
                PolicyNumber = "POL000000000",
                Amount = 999.99m
            };

            var newPolicyId = await policyRepo.CreatePolicyAsync(newPolicy);
            Console.WriteLine($"New policy created with ID: {newPolicyId}");

            Console.WriteLine("\nFetching all policies...");
            var policies = await policyRepo.GetAllPoliciesAsync();
            foreach (var policy in policies)
            {
                Console.WriteLine($"Policy ID: {policy.PolicyId}, Amount: {policy.Amount}");
            }

            Console.WriteLine("\nFetching policy by ID...");
            var fetchedPolicy = await policyRepo.GetPolicyByIdAsync(newPolicyId);

            Console.WriteLine(fetchedPolicy != null
                ? $"Fetched Policy ID: {fetchedPolicy.PolicyId}, Number: {fetchedPolicy.PolicyNumber}, Amount: {fetchedPolicy.Amount}"
                : "Policy not found.");

            Console.WriteLine("\nUpdating the newly created policy...");
            if (fetchedPolicy != null)
            {
                fetchedPolicy.PolicyNumber = "POL999999999";
                fetchedPolicy.Amount = 150.75m;
                var updateResult = await policyRepo.UpdatePolicyAsync(fetchedPolicy);
                Console.WriteLine(updateResult ? "Policy updated successfully." : "Failed to update policy.");
            }

            Console.WriteLine("\nDeleting the policy...");
            var (isDeleted, deleteMessage) = await policyRepo.DeletePolicyAsync(newPolicyId);
            Console.WriteLine(deleteMessage);

            if (isDeleted)
            {
                Console.WriteLine("\nFetching all policies after deletion...");
                var policiesAfterDeletion = await policyRepo.GetAllPoliciesAsync();
                foreach (var policy in policiesAfterDeletion)
                {
                    Console.WriteLine($"Policy ID: {policy.PolicyId}, Number: {policy.PolicyNumber}, Amount: {policy.Amount}");
                }
            }

            Console.WriteLine("\nDeleting the policy connected to the partner...");
            var (isDeleted2, deleteMessage2) = await policyRepo.DeletePolicyAsync(4);
            Console.WriteLine(deleteMessage2);

            if (isDeleted2)
            {
                Console.WriteLine("\nFetching all policies after deletion...");
                var policiesAfterDeletion = await policyRepo.GetAllPoliciesAsync();
                foreach (var policy in policiesAfterDeletion)
                {
                    Console.WriteLine($"Policy ID: {policy.PolicyId}, Number: {policy.PolicyNumber}, Amount: {policy.Amount}");
                }
            }

            Console.WriteLine("Assign policy to partner...");
            await policyRepo.AssignPolicyToPartnerAsync(3, 6);
            await policyRepo.AssignPolicyToPartnerAsync(1, 5);

            Console.WriteLine("Remove policy from partner...");
            var policyId = 3;
            var partnerId = 6;
            var isRemoved = await policyRepo.RemovePolicyFromPartnerAsync(policyId, partnerId);
            Console.WriteLine(isRemoved
                ? $"Successfully removed Policy ID {policyId} from Partner ID {partnerId}."
                : $"Failed to remove Policy ID {policyId} from Partner ID {partnerId}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

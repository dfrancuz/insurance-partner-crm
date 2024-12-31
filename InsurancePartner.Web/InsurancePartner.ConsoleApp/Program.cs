namespace InsurancePartner.ConsoleApp;

using Data.DependencyInjection;
using Logic.DependencyInjection;
using Logic.DTOs;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            var services = new ServiceCollection();

            services.AddDataLayer(configuration);
            services.AddLogicLayer();

            await using var serviceProvider = services.BuildServiceProvider();

            var partnerService = serviceProvider.GetRequiredService<IPartnerService>();
            var policyService = serviceProvider.GetRequiredService<IPolicyService>();

            // 1. Create new policy
            var newPolicy = new CreatePolicyDto
            {
                PolicyNumber = "POL987789000",
                Amount = 10.00m
            };

            var createResult = await policyService.CreatePolicyAsync(newPolicy);
            Console.WriteLine($"Create policy result: {createResult.Message}");

            // 2. List all policies
            var policies = await policyService.GetAllPoliciesAsync();
            foreach (var policy in policies)
            {
                Console.WriteLine($"Policy: {policy.PolicyId} - {policy.PolicyNumber}, Amount: {policy.Amount}");
            }

            // 3. List policies for specific partner
            var partnerPolicies = await policyService.GetPartnerPoliciesAsync(10);
            Console.WriteLine($"\nPolicies for partner 10:");

            foreach (var policy in partnerPolicies)
            {
                Console.WriteLine($"Policy: {policy.PolicyId} - {policy.PolicyNumber}, Amount: {policy.Amount}");
            }

            // 4. Try to create invalid policy
            var invalidPolicy = new CreatePolicyDto
            {
                PolicyNumber = "12",
                Amount = 99.00m
            };

            var invalidResult = await policyService.CreatePolicyAsync(invalidPolicy);
            Console.WriteLine($"\nInvalid policy test result: {invalidResult.Message}");

            var getSinglePartner = await partnerService.GetPartnerByIdAsync(10);
            Console.WriteLine(
                $"\nPartner: {getSinglePartner.FirstName} - {getSinglePartner.LastName}, " +
                $"{getSinglePartner.PartnerNumber}" +
                $"Policies: {string.Join(", ", getSinglePartner.Policies.Select(p => p.PolicyNumber))}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

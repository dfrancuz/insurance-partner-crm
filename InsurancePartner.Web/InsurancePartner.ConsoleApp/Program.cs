namespace InsurancePartner.ConsoleApp;

using Data.Models;
using Logic.DTOs;
using Logic.Mappers;

public static class Program
{
    public static void Main()
    {
        Test();
    }

    private static void Test()
    {
        var policy = new Policy
        {
            PolicyId = 1,
            PolicyNumber = "P123",
            Amount = 1000,
            CreatedAtUtc = DateTime.UtcNow
        };

        var partner = new Partner
        {
            PartnerId = 1,
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "PN123",
            CroatianPIN = "12345678901",
            PartnerTypeId = 1,
            CreatedAtUtc = DateTime.UtcNow,
            CreateByUser = "admin@admin.com",
            IsForeign = false,
            ExternalCode = "EXT123",
            Gender = 'M',
            Policies =
            [
                new Policy { PolicyId = 1, PolicyNumber = "P123", Amount = 1000, CreatedAtUtc = DateTime.UtcNow },
                new Policy { PolicyId = 2, PolicyNumber = "P124", Amount = 2000, CreatedAtUtc = DateTime.UtcNow }
            ]
        };

        var createPartnerDto = new CreatePartnerDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            PartnerNumber = "PN456",
            CroatianPIN = "98765432109",
            PartnerTypeId = 2,
            CreateByUser = "user1@example.com",
            IsForeign = true,
            ExternalCode = "EXT456",
            Gender = 'F'
        };

        var invalidPartnerDto = new CreatePartnerDto
        {
            FirstName = "Invalid",
            PartnerNumber = "PN999",
            CroatianPIN = "12345678901",
            PartnerTypeId = 1,
            CreateByUser = "admin@admin.com",
            IsForeign = false,
            ExternalCode = "EXT999",
            Gender = 'M'
        };

        var partnerWithManyPolicies = new Partner
        {
            PartnerId = 3,
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "PN1000",
            CroatianPIN = "12345678902",
            PartnerTypeId = 3,
            CreatedAtUtc = DateTime.UtcNow,
            CreateByUser = "someone@at.com",
            IsForeign = false,
            ExternalCode = "EXT1000",
            Gender = 'M',
            Policies = new List<Policy>()
        };

        var createPolicyDto = new CreatePolicyDto
        {
            PolicyNumber = "P125",
            Amount = 5000
        };

        try
        {
            // Test Case 1: Valid partner object
            var partnerDto = PartnerMapper.ToDto(partner);
            Console.WriteLine($"Partner DTO: PartnerId: {partnerDto.PartnerId}, Full Name: {partnerDto.FullName}, Policies Count: {partnerDto.Policies.Count}");

            // Test Case 2: Null partner object (should return null)
            Partner nullPartner = null;
            var nullPartnerDto = PartnerMapper.ToDto(nullPartner);
            Console.WriteLine($"Null Partner DTO: {nullPartnerDto}");

            // Test Case 3: Valid policy object
            var policyDto = PolicyMapper.ToDto(policy);
            Console.WriteLine($"Policy DTO: PolicyId: {policyDto.PolicyId}, PolicyNumber: {policyDto.PolicyNumber}, Amount: {policyDto.Amount}");

            // Test Case 4: Null policy object (should return null)
            Policy nullPolicy = null;
            var nullPolicyDto = PolicyMapper.ToDto(nullPolicy);
            Console.WriteLine($"Null Policy DTO: {nullPolicyDto}");

            // Test Case 5: Valid CreatePartnerDto to Partner conversion
            var partnerFromDto = PartnerMapper.ToEntity(createPartnerDto);
            Console.WriteLine($"Partner from DTO: {partnerFromDto.FirstName} {partnerFromDto.LastName}");

            // Test Case 6: Valid CreatePolicyDto to Policy conversion
            var policyFromDto = PolicyMapper.ToEntity(createPolicyDto);
            Console.WriteLine($"Policy from DTO: {policyFromDto.PolicyNumber}, Amount: {policyFromDto.Amount}");

            // Test Case 7: Invalid DTO with missing required fields
            var partnerFromInvalidDto = PartnerMapper.ToEntity(invalidPartnerDto);
            Console.WriteLine($"Partner from Invalid DTO: {partnerFromInvalidDto.FirstName} {partnerFromInvalidDto.LastName}");

            // Test Case 8: Partner with a large number of policies
            for (var i = 0; i < 1000; i++)
            {
                partnerWithManyPolicies.Policies.Add(new Policy
                {
                    PolicyId = i + 1,
                    PolicyNumber = $"P{i + 1}",
                    Amount = 1000 + (i * 100),
                    CreatedAtUtc = DateTime.UtcNow
                });
            }

            var partnerWithManyPoliciesDto = PartnerMapper.ToDto(partnerWithManyPolicies);
            Console.WriteLine($"Partner with Many Policies DTO: PartnerId: {partnerWithManyPoliciesDto.PartnerId}, " +
                              $"Policies Count: {partnerWithManyPoliciesDto.Policies.Count}, " +
                              $"Total Amount: {partnerWithManyPoliciesDto.Policies.Sum(p => p.Amount)}, " +
                              $"Is high risk? {partnerWithManyPoliciesDto.IsHighRisk}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

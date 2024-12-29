using InsurancePartner.Data.Models;
using InsurancePartner.Logic.DTOs;

namespace InsurancePartner.Logic.Mappers;

public class PolicyMapper
{
    public static PolicyDto ToDto(Policy? policy)
    {
        if (policy == null)
        {
            return null;
        }

        return new PolicyDto
        {
            PolicyId = policy.PolicyId,
            PolicyNumber = policy.PolicyNumber,
            Amount = policy.Amount,
            CreatedAtUtc = policy.CreatedAtUtc
        };
    }

    public static Policy ToEntity(CreatePolicyDto policyDto)
    {
        return new Policy
        {
            PolicyNumber = policyDto.PolicyNumber,
            Amount = policyDto.Amount,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}

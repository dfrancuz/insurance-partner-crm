namespace InsurancePartner.Logic.Mappers;

using Data.Models;
using DTOs;

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

    public static Policy ToEntity(PolicyDto policyDto)
    {
        return new Policy
        {
            PolicyId = policyDto.PolicyId,
            PolicyNumber = policyDto.PolicyNumber,
            Amount = policyDto.Amount,
            CreatedAtUtc = policyDto.CreatedAtUtc
        };
    }

    public static Policy ToEntity(CreatePolicyDto policyDto)
    {
        if (policyDto == null)
        {
            return null;
        }

        return new Policy
        {
            PolicyNumber = policyDto.PolicyNumber,
            Amount = policyDto.Amount,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}

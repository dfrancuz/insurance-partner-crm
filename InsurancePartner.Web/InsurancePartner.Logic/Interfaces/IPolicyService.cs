namespace InsurancePartner.Logic.Interfaces;

using DTOs;

public interface IPolicyService
{
    Task<IEnumerable<PolicyDto>> GetAllPoliciesAsync();

    Task<PolicyDto?> GetPolicyByIdAsync(int policyId);

    Task<(bool IsSuccess, string Message)> CreatePolicyAsync(CreatePolicyDto policyDto);

    Task<(bool IsSuccess, string Message)> UpdatePolicyAsync(UpdatePolicyDto policyDto);

    Task<(bool IsSuccess, string Message)> DeletePolicyAsync(int policyId);
}

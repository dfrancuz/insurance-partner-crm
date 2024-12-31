namespace InsurancePartner.Logic.Interfaces;

using DTOs;

public interface IPolicyService
{
    Task<IEnumerable<PolicyDto>> GetAllPoliciesAsync();

    Task<PolicyDto?> GetPolicyByIdAsync(int policyId);

    Task<(bool IsSuccess, string Message)> CreatePolicyAsync(CreatePolicyDto policyDto);

    Task<(bool IsSuccess, string Message)> UpdatePolicyAsync(PolicyDto policyDto);

    Task<(bool IsSuccess, string Message)> DeletePolicyAsync(int policyId);

    Task<int> AssignPolicyToPartnerAsync(int policyId, int partnerId);

    Task<IEnumerable<PolicyDto>> GetPartnerPoliciesAsync(int partnerId);

    Task<bool> RemovePolicyFromPartnerAsync(int policyId, int partnerId);

    Task<bool> PolicyNumberExistsAsync(string policyNumber);
}

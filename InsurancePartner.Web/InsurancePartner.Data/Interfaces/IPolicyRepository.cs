using InsurancePartner.Data.Models;

namespace InsurancePartner.Data.Interfaces;

public interface IPolicyRepository
{
    Task<IEnumerable<Policy>> GetAllPoliciesAsync();

    Task<Policy?> GetPolicyByIdAsync(int policyId);

    Task<int> CreatePolicyAsync(Policy policy);

    Task<bool> UpdatePolicyAsync(Policy policy);

    Task<(bool IsDeleted, string message)> DeletePolicyAsync(int policyId);

    Task<int> AssignPolicyToPartnerAsync(int policyId, int partnerId);

    Task<IEnumerable<Policy>> GetPartnerPoliciesAsync(int partnerId);

    Task<bool> RemovePolicyFromPartnerAsync(int policyId, int partnerId);

    Task<bool> PolicyNumberExistsAsync(string policyNumber);
}

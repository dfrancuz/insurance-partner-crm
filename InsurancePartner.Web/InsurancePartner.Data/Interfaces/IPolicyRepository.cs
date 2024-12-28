using InsurancePartner.Data.Models;

namespace InsurancePartner.Data.Interfaces;

public interface IPolicyRepository
{
    Task<int> CreatePolicyAsync(Policy policy);

    Task<int> AssignPolicyToPartnerAsync(int policyId, int partnerId);

    Task<IEnumerable<Policy>> GetPartnerPoliciesAsync(int partnerId);

    Task<bool> RemovePolicyFromPartnerAsync(int policyId, int partnerId);

    Task<bool> PolicyNumberExistsAsync(string policyNumber);
}

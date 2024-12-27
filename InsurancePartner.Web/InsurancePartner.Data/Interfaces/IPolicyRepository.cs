using InsurancePartner.Data.Models;

namespace InsurancePartner.Data.Interfaces;

public interface IPolicyRepository
{
    Task<int> CreatePolicyAsync(Policy policy);

    Task<IEnumerable<Policy>> GetPartnerPoliciesAsync(int partnerId);

    Task<bool> PolicyNumberExistsAsync(string policyNumber);
}

namespace InsurancePartner.Logic.Services;

using DTOs;
using InsurancePartner.Data.Interfaces;
using Interfaces;
using Mappers;
using Validators;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository _policyRepository;
    private readonly CreatePolicyValidator _createPolicyValidator;
    private readonly UpdatePolicyValidator _updatePolicyValidator;

    public PolicyService(IPolicyRepository policyRepository, CreatePolicyValidator createPolicyValidator, UpdatePolicyValidator updatePolicyValidator)
    {
        _policyRepository = policyRepository;
        _createPolicyValidator = createPolicyValidator;
        _updatePolicyValidator = updatePolicyValidator;
    }

    public async Task<IEnumerable<PolicyDto>> GetAllPoliciesAsync()
    {
        var policies = await _policyRepository.GetAllPoliciesAsync();
        return policies.Select(PolicyMapper.ToDto);
    }

    public async Task<PolicyDto?> GetPolicyByIdAsync(int policyId)
    {
        var policy = await _policyRepository.GetPolicyByIdAsync(policyId);
        return policy != null ? PolicyMapper.ToDto(policy) : null;
    }

    public async Task<(bool IsSuccess, string Message)> CreatePolicyAsync(CreatePolicyDto policyDto)
    {
        var validationResult = await _createPolicyValidator.ValidateAsync(policyDto);

        if (!validationResult.IsValid)
        {
            return (false, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        if (await _policyRepository.PolicyNumberExistsAsync(policyDto.PolicyNumber))
        {
            return (false, $"Policy number already exists: {policyDto.PolicyNumber}");
        }

        try
        {
            var policy = PolicyMapper.ToEntity(policyDto);
            await _policyRepository.CreatePolicyAsync(policy);
            return (true, "Policy created successfully");
        }
        catch (Exception e)
        {
            return (false, $"Error creating policy: {e.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Message)> UpdatePolicyAsync(UpdatePolicyDto policyDto)
    {
        var validationResult = await _updatePolicyValidator.ValidateAsync(policyDto);

        if (!validationResult.IsValid)
        {
            return (false, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        try
        {
            var policy = PolicyMapper.ToEntity(policyDto);
            var success = await _policyRepository.UpdatePolicyAsync(policy);
            return success
                ? (true, "Policy updated successfully")
                : (false, $"Policy not found");

        }
        catch (Exception e)
        {
            return (false, $"Error updating policy: {e.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Message)> DeletePolicyAsync(int policyId)
    {
        try
        {
            var (isDeleted, message) = await _policyRepository.DeletePolicyAsync(policyId);
            return isDeleted
                ? (true, "Policy deleted successfully")
                : (false, message);
        }
        catch (Exception e)
        {
            return (false, $"Error deleting policy: {e.Message}");
        }
    }
}

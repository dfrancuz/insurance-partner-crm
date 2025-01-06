using InsurancePartner.Data.Models;

namespace InsurancePartner.Logic.Services;

using DTOs;
using InsurancePartner.Data.Interfaces;
using Interfaces;
using Mappers;
using Validators;

public class PartnerService : IPartnerService
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPolicyRepository _policyRepository;
    private readonly CreatePartnerValidator _createPartnerValidator;
    private readonly UpdatePartnerValidator _updatePartnerValidator;

    public PartnerService(
        IPartnerRepository partnerRepository,
        IPolicyRepository policyRepository,
        CreatePartnerValidator createPartnerValidator,
        UpdatePartnerValidator updatePartnerValidator)
    {
        _partnerRepository = partnerRepository;
        _policyRepository = policyRepository;
        _createPartnerValidator = createPartnerValidator;
        _updatePartnerValidator = updatePartnerValidator;
    }

    public async Task<IEnumerable<PartnerDto>> GetAllPartnersAsync()
    {
        var partners = await _partnerRepository.GetAllPartnersAsync();
        return partners.Select(PartnerMapper.ToDto);
    }

    public async Task<PartnerDto?> GetPartnerByIdAsync(int partnerId)
    {
        var partner = await _partnerRepository.GetPartnerByIdAsync(partnerId);
        return partner != null ? PartnerMapper.ToDto(partner) : null;
    }

    public async Task<(bool IsSuccess, string Message)> CreatePartnerAsync(CreatePartnerDto partnerDto)
    {
        var validationResult = await _createPartnerValidator.ValidateAsync(partnerDto);

        if (!validationResult.IsValid)
        {
            return (false, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var partner = PartnerMapper.ToEntity(partnerDto);
        var partnerId = await _partnerRepository.CreatePartnerAsync(partner);

        if (partnerDto.SelectedPolicyIds.Any())
        {
            foreach (var policyId in partnerDto.SelectedPolicyIds)
            {
                var result = await _policyRepository.AssignPolicyToPartnerAsync(policyId, partnerId);
                if (result < 0)
                {
                    return (false, "Failed to assign policy to partner.");
                }
            }
        }
        return partnerId > 0
            ? (true, "Partner created successfully.")
            : (false, "Failed to create partner.");
    }

    public async Task<(bool IsSuccess, string Message)> UpdatePartnerAsync(UpdatePartnerDto partnerDto)
    {
        var validationResult = await _updatePartnerValidator.ValidateAsync(partnerDto);

        if (!validationResult.IsValid)
        {
            return (false, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var existingPartner = await _partnerRepository.GetPartnerByIdAsync(partnerDto.PartnerId);

        if (existingPartner == null)
        {
            return (false, "Partner not found.");
        }

        var existingPolicies = (await _policyRepository.GetPartnerPoliciesAsync(partnerDto.PartnerId)).ToList();

        var policiesToRemove = existingPolicies
            .Where(p => !partnerDto.SelectedPolicyIds.Contains(p.PolicyId))
            .ToList();

        foreach (var policy in policiesToRemove)
        {
            await _policyRepository.RemovePolicyFromPartnerAsync(policy.PolicyId, partnerDto.PartnerId);
        }

        var selectedPolicyIdsToAdd = partnerDto.SelectedPolicyIds
            .Where(id => !existingPolicies.Any(p => p.PolicyId == id))
            .ToList();

        foreach (var policyId in selectedPolicyIdsToAdd)
        {
            await _policyRepository.AssignPolicyToPartnerAsync(policyId, partnerDto.PartnerId);
        }

        var partner = PartnerMapper.ToEntity(partnerDto, existingPolicies);
        var updateResult = await _partnerRepository.UpdatePartnerAsync(partner);

        return !updateResult
            ? (false, "Failed to update partner.")
            : (true, "Partner updated successfully.");
    }

    public async Task<(bool IsSuccess, string Message)> DeletePartnerAsync(int partnerId)
    {
        var partner = await _partnerRepository.GetPartnerByIdAsync(partnerId);

        if (partner == null)
        {
            return (false, "Partner not found.");
        }

        if (partner.Policies.Any())
        {
            return (
                false,
                $"Cannot delete the partner because they have {partner.Policies.Count} associated policies. " +
                $"Please unassign or remove them before deleting partner.");
        }

        var deleted = await _partnerRepository.DeletePartnerAsync(partnerId);

        return deleted
            ? (true, "Partner deleted successfully")
            : (false, "Failed to delete partner.");
    }

    public async Task<IEnumerable<PartnerTypeDto>> GetPartnerTypesAsync()
    {
        var types = await _partnerRepository.GetPartnerTypesAsync();
        return types.Select(PartnerTypeMapper.ToDto);
    }
}

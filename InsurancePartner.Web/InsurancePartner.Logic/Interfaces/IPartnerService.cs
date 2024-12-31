namespace InsurancePartner.Logic.Interfaces;

using DTOs;

public interface IPartnerService
{
    Task<IEnumerable<PartnerDto>> GetAllPartnersAsync();

    Task<PartnerDto?> GetPartnerByIdAsync(int partnerId);

    Task<(bool IsSuccess, string Message)> CreatePartnerAsync(CreatePartnerDto partnerDto);

    Task<(bool IsSuccess, string Message)> UpdatePartnerAsync(PartnerDto partnerDto);

    Task<(bool IsSuccess, string Message)> DeletePartnerAsync(int partnerId);

    Task<IEnumerable<PartnerTypeDto>> GetPartnerTypesAsync();

    Task<bool> ExternalCodeExistsAsync(string externalCode);

    Task<bool> PartnerNumberExistsAsync(string partnerNumber);
}

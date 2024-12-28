namespace InsurancePartner.Data.Interfaces;

using Models;

public interface IPartnerRepository
{
    Task<IEnumerable<Partner>> GetAllPartnersAsync();

    Task<Partner?> GetPartnerByIdAsync(int partnerId);

    Task<int> CreatePartnerAsync(Partner partner);

    Task<bool> UpdatePartnerAsync(Partner partner);

    Task<bool> DeletePartnerAsync(int partnerId);

    Task<IEnumerable<PartnerType>> GetPartnerTypesAsync();

    Task<bool> ExternalCodeExistsAsync(string externalCode);

    Task<bool> PartnerNumberExistsAsync(string partnerNumber);
}

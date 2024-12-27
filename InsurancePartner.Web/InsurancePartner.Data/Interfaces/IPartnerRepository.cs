using InsurancePartner.Data.Models;

namespace InsurancePartner.Data.Interfaces;

public interface IPartnerRepository
{
    Task<IEnumerable<Partner>> GetAllPartnersAsync();

    Task<Partner?> GetPartnerByIdAsync(int partnerId);

    Task<int> CreatePartnerAsync(Partner partner);

    Task<IEnumerable<PartnerType>> GetPartnerTypesAsync();

    Task<bool> ExternalCodeExistsAsync(string externalCode);

    Task<bool> PartnerNumberExistsAsync(string partnerNumber);
}

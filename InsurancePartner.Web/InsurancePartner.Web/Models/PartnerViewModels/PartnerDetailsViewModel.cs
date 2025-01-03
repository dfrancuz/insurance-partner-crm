using InsurancePartner.Logic.DTOs;

namespace InsurancePartner.Web.Models.PartnerViewModels;

public class PartnerDetailsViewModel
{
    public int PartnerId { get; set; }

    public string FullName { get; set; }

    public string Address { get; set; }

    public string PartnerNubmer { get; set; }

    public string CroatianPIN { get; set; }

    public int PartnerTypeId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public string CreateByUser { get; set; }

    public bool IsForeign { get; set; }

    public string ExternalCode { get; set; }

    public char Gender { get; set; }

    public IEnumerable<PolicyDto> Policies { get; set; }
}

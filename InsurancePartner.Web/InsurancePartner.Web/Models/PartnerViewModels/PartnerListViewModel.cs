namespace InsurancePartner.Web.Models.PartnerViewModels;

public class PartnerListViewModel
{
    public int PartnerId { get; set; }

    public string FullName { get; set; }

    public string PartnerNumber { get; set; }

    public string CroatianPIN { get; set; }

    public int PartnerTypeId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public bool IsForeign { get; set; }

    public char Gender { get; set; }

    public int TotalPolicies { get; set; }

    public decimal TotalPolicyAmount { get; set; }

    public bool HasSpecialStatus { get; set; }
}

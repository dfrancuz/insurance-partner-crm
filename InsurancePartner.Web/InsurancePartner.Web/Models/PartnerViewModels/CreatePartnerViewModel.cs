namespace InsurancePartner.Web.Models.PartnerViewModels;

using Logic.DTOs;

public class CreatePartnerViewModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Address { get; set; }

    public string PartnerNumber { get; set; }

    public string? CroatianPIN { get; set; }

    public int PartnerTypeId { get; set; }

    public string CreateByUser { get; set; }

    public bool IsForeign { get; set; }

    public string ExternalCode { get; set; }

    public char Gender { get; set; }

    public List<PartnerTypeDto> PartnerTypes { get; set; } = [];

    public List<PolicyDto> AvailablePolicies { get; set; } = [];

    public List<int> SelectedPolicyIds { get; set; } = [];
}

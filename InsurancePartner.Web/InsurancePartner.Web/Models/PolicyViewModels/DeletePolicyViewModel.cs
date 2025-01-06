namespace InsurancePartner.Web.Models.PolicyViewModels;

public class DeletePolicyViewModel
{
    public int PolicyId { get; set; }

    public string PolicyNumber { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}

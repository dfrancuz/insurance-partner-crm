namespace InsurancePartner.Data.Models;

public class Policy
{
    public int PolicyId { get; set; }

    public string PolicyNumber { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}

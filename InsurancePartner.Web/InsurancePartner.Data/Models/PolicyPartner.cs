namespace InsurancePartner.Data.Models;

public class PolicyPartner
{
    public int PolicyPartnerId { get; set; }

    public int PolicyId { get; set; }

    public int PartnerId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public Policy Policy { get; set; }

    public Partner Partner { get; set; }
}

namespace InsurancePartner.Logic.DTOs;

public class PartnerDto
{
    public int PartnerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public string? Address { get; set; }

    public string PartnerNumber { get; set; }

    public string? CroatianPIN { get; set; }

    public int PartnerTypeId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public string CreateByUser { get; set; }

    public bool IsForeign { get; set; }

    public string ExternalCode { get; set; }

    public char Gender { get; set; }

    public List<PolicyDto> Policies { get; set; }

    public int PolicyCount { get; set; }

    public decimal TotalPolicyAmount { get; set; }
}

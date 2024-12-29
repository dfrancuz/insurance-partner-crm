using InsurancePartner.Data.Models;
using InsurancePartner.Logic.DTOs;

namespace InsurancePartner.Logic.Mappers;

public class PartnerMapper
{
    public static PartnerDto ToDto(Partner? partner)
    {
        if (partner == null)
        {
            return null;
        }

        return new PartnerDto
        {
            PartnerId = partner.PartnerId,
            FirstName = partner.FirstName,
            LastName = partner.LastName,
            Address = partner.Address,
            PartnerNumber = partner.PartnerNumber,
            CroatianPIN = partner.CroatianPIN,
            PartnerTypeId = partner.PartnerTypeId,
            CreatedAtUtc = partner.CreatedAtUtc,
            CreateByUser = partner.CreateByUser,
            IsForeign = partner.IsForeign,
            ExternalCode = partner.ExternalCode,
            Gender = partner.Gender,
            Policies = partner.Policies?.Select(PolicyMapper.ToDto).ToList() ?? []
        };
    }

    public static Partner ToEntity(CreatePartnerDto partnerDto)
    {
        return new Partner
        {
            FirstName = partnerDto.FirstName,
            LastName = partnerDto.LastName,
            Address = partnerDto.Address,
            PartnerNumber = partnerDto.PartnerNumber,
            CroatianPIN = partnerDto.CroatianPIN,
            PartnerTypeId = partnerDto.PartnerTypeId,
            CreateByUser = partnerDto.CreateByUser,
            IsForeign = partnerDto.IsForeign,
            ExternalCode = partnerDto.ExternalCode,
            Gender = partnerDto.Gender,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}

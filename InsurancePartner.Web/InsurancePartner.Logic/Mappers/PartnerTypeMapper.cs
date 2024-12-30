namespace InsurancePartner.Logic.Mappers;

using Data.Models;
using DTOs;

public class PartnerTypeMapper
{
    public static PartnerTypeDto ToDto(PartnerType? partnerType)
    {
        if (partnerType == null)
        {
            return null;
        }

        return new PartnerTypeDto
        {
            PartnerTypeId = partnerType.PartnerTypeId,
            TypeName = partnerType.TypeName
        };
    }

    public static PartnerType ToEntity(PartnerTypeDto partnerTypeDto)
    {
        return new PartnerType()
        {
            PartnerTypeId = partnerTypeDto.PartnerTypeId,
            TypeName = partnerTypeDto.TypeName
        };
    }
}

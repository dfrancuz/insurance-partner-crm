namespace InsurancePartner.Logic.Validators;

using FluentValidation;
using InsurancePartner.Data.Interfaces;
using DTOs;

public class UpdatePartnerValidator : AbstractValidator<PartnerDto>
{
    public UpdatePartnerValidator(IPartnerRepository partnerRepository)
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 255).WithMessage("First name must be between 2 and 255 characters");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 255).WithMessage("Last name must be between 2 and 255 characters");

        RuleFor(p => p.Address)
            .Length(0, 500).WithMessage("Address shouldn't be longer than 500 characters");

        RuleFor(p => p.PartnerNumber)
            .NotEmpty().WithMessage("Partner number is required")
            .Must(pn => pn.All(char.IsDigit)).WithMessage("Partner number must contain only digits.")
            .Length(20).WithMessage("Partner number must be exactly 20 characters")
            .MustAsync(async (dto, number, _) =>
            {
                var existingPartner = await partnerRepository.GetPartnerByIdAsync(dto.PartnerId);
                return existingPartner == null
                       || existingPartner.PartnerNumber == number
                       || !await partnerRepository.PartnerNumberExistsAsync(number);
            }).WithMessage("Partner number must be unique if updated");

        RuleFor(p => p.CroatianPIN)
            .Matches(@"^\d{11}$").When(p => !string.IsNullOrEmpty(p.CroatianPIN))
            .WithMessage("Croatian PIN (OIB) must be exactly 11 digits");

        RuleFor(p => p.PartnerTypeId)
            .InclusiveBetween(1, 2).WithMessage("Partner type must be either 1 (Personal) or 2 (Legal)");

        RuleFor(p => p.CreateByUser)
            .NotEmpty().WithMessage("Email is required")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Must be valid email address");

        RuleFor(p => p.IsForeign)
            .NotEmpty().WithMessage("Is foreign must be true or false");

        RuleFor(p => p.ExternalCode)
            .Length(10, 20).WithMessage("External code must be between 10 and 20 characters long")
            .MustAsync(async (dto, code, _) =>
            {
                var existingPartner = await partnerRepository.GetPartnerByIdAsync(dto.PartnerId);
                return existingPartner == null
                       || existingPartner.ExternalCode == code
                       || !await partnerRepository.ExternalCodeExistsAsync(code);
            }).WithMessage("External code must be unique if updated");

        RuleFor(p => p.Gender)
            .Must(ch => ch is 'M' or 'F' or 'N').WithMessage("Gender must be chosen correctly");
    }
}

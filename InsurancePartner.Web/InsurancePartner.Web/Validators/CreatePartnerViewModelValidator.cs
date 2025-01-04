using FluentValidation;
using InsurancePartner.Web.Models.PartnerViewModels;

namespace InsurancePartner.Web.Validators;

public class CreatePartnerViewModelValidator : AbstractValidator<CreatePartnerViewModel>
{
    public CreatePartnerViewModelValidator()
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
            .Length(20).WithMessage("Partner number must be exactly 20 characters");

        RuleFor(p => p.CroatianPIN)
            .Matches(@"^\d{11}$").When(p => !string.IsNullOrEmpty(p.CroatianPIN))
            .WithMessage("Croatian PIN (OIB) must be exactly 11 digits");

        RuleFor(p => p.CreateByUser)
            .NotEmpty().WithMessage("Email is required")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Must be valid email address");

        RuleFor(p => p.ExternalCode)
            .NotEmpty().WithMessage("External code is required")
            .Length(10, 20).WithMessage("External code must be between 10 and 20 characters");
    }
}

namespace InsurancePartner.Web.Validators;

using FluentValidation;
using Models.PolicyViewModels;

public class CreatePolicyViewModelValidator : AbstractValidator<CreatePolicyViewModel>
{
    public CreatePolicyViewModelValidator()
    {
        RuleFor(p => p.PolicyNumber)
            .NotEmpty().WithMessage("Policy number is required.")
            .Length(10, 15).WithMessage("Policy number must be between 10 and 15 characters.");

        RuleFor(p => p.Amount)
            .NotEmpty().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .LessThanOrEqualTo(999999999999.99M).WithMessage("Amount should be in range 0 - 999,999,999,999.99.");
    }
}

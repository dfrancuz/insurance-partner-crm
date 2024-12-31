namespace InsurancePartner.Logic.Validators;

using DTOs;
using FluentValidation;
using InsurancePartner.Data.Interfaces;

public class CreatePolicyValidator : AbstractValidator<CreatePolicyDto>
{
    public CreatePolicyValidator(IPolicyRepository policyRepository)
    {
        RuleFor(p => p.PolicyNumber)
            .NotEmpty().WithMessage("Policy number is required")
            .Length(10, 15).WithMessage("Policy number must be between 10 and 15 characters")
            .MustAsync(async (number, _) => !await policyRepository.PolicyNumberExistsAsync(number))
            .WithMessage("Policy number already exists");

        RuleFor(p => p.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
    }
}

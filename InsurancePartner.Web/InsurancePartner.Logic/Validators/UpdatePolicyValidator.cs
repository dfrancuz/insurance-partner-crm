namespace InsurancePartner.Logic.Validators;

using DTOs;
using FluentValidation;
using InsurancePartner.Data.Interfaces;

public class UpdatePolicyValidator : AbstractValidator<UpdatePolicyDto>
{
    public UpdatePolicyValidator(IPolicyRepository policyRepository)
    {
        RuleFor(p => p.PolicyNumber)
            .NotEmpty().WithMessage("Policy number is required")
            .Length(10, 15).WithMessage("Policy number must be between 10 and 15 characters")
            .MustAsync(async (dto, number, _) =>
            {
                var existingPolicy = await policyRepository.GetPolicyByIdAsync(dto.PolicyId);
                return existingPolicy == null
                       || existingPolicy.PolicyNumber == number
                       || !await policyRepository.PolicyNumberExistsAsync(number);
            }).WithMessage("Policy number must be unique if updated.");

        RuleFor(p => p.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
    }
}

namespace InsurancePartner.Logic.DependencyInjection;

using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Validators;

public static class LogicLayerServiceCollectionExtensions
{
    public static IServiceCollection AddLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<CreatePartnerValidator>();
        services.AddScoped<UpdatePartnerValidator>();
        services.AddScoped<CreatePolicyValidator>();
        services.AddScoped<UpdatePolicyValidator>();

        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<IPolicyService, PolicyService>();

        return services;
    }
}

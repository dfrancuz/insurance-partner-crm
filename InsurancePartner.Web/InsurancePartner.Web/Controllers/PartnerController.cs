using InsurancePartner.Logic.Interfaces;
using InsurancePartner.Web.Models.PartnerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePartner.Web.Controllers;

public class PartnerController : Controller
{
    private readonly IPartnerService _partnerService;
    private readonly IPolicyService _policyService;

    public PartnerController(IPartnerService partnerService, IPolicyService policyService)
    {
        _partnerService = partnerService;
        _policyService = policyService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var partners = await _partnerService.GetAllPartnersAsync();
        var viewModels = partners.Select(p => new PartnerListViewModel
        {
            PartnerId = p.PartnerId,
            FullName = p.FullName,
            PartnerNumber = p.PartnerNumber,
            CroatianPIN = p.CroatianPIN,
            PartnerTypeId = p.PartnerTypeId,
            CreatedAtUtc = p.CreatedAtUtc,
            IsForeign = p.IsForeign,
            Gender = p.Gender,
            TotalPolicies = p.Policies?.Count ?? 0,
            TotalPolicyAmount = p.Policies?.Sum(x => x.Amount) ?? 0,
            HasSpecialStatus = (p.Policies?.Count ?? 0) > 5 || (p.Policies?.Sum(x => x.Amount) ?? 0) > 5000,
        }).OrderByDescending(p => p.CreatedAtUtc);

        return View(viewModels);
    }
}

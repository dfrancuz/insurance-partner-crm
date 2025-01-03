namespace InsurancePartner.Web.Controllers;

using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.PartnerViewModels;

[Route("partners")]
public class PartnersController : Controller
{
    private readonly IPartnerService _partnerService;
    private readonly IPolicyService _policyService;

    public PartnersController(IPartnerService partnerService, IPolicyService policyService)
    {
        _partnerService = partnerService;
        _policyService = policyService;
    }

    // GET /partners
    [HttpGet]
    public async Task<IActionResult> PartnerIndex()
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

    [HttpGet("{partnerId}")]
    public async Task<IActionResult> Details(int partnerId)
    {
        var partner = await _partnerService.GetPartnerByIdAsync(partnerId);

        if (partner == null)
        {
            return NotFound();
        }

        var viewModel = new PartnerDetailsViewModel
        {
            PartnerId = partner.PartnerId,
            FullName = partner.FullName,
            Address = partner.Address,
            PartnerNubmer = partner.PartnerNumber,
            CroatianPIN = partner.CroatianPIN,
            PartnerTypeId = partner.PartnerTypeId,
            CreatedAtUtc = partner.CreatedAtUtc,
            CreateByUser = partner.CreateByUser,
            IsForeign = partner.IsForeign,
            ExternalCode = partner.ExternalCode,
            Gender = partner.Gender,
            Policies = partner.Policies
        };

        return PartialView("_PartnerDetails", viewModel);
    }
}

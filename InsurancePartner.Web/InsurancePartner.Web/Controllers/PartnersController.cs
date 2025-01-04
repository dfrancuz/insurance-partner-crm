namespace InsurancePartner.Web.Controllers;

using Logic.DTOs;
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

    // GET /partners/{id}
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

    // GET /partners/create
    [HttpGet("create")]
    public async Task<IActionResult> PartnerCreate()
    {
        var partnerTypes = await _partnerService.GetPartnerTypesAsync();
        var policies = await _policyService.GetAllPoliciesAsync();

        var viewModel = new CreatePartnerViewModel
        {
            PartnerTypes = partnerTypes.ToList(),
            AvailablePolicies = policies.ToList()
        };

        return View(viewModel);
    }

    // POST /partners/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PartnerCreate(CreatePartnerViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.PartnerTypes = (await _partnerService.GetPartnerTypesAsync()).ToList();
            viewModel.AvailablePolicies = (await _policyService.GetAllPoliciesAsync()).ToList();

            ModelState.AddModelError("", "There are some issues with your input. Please check all fields and try again.");

            return View(viewModel);
        }

        var createPartnerDto = new CreatePartnerDto
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Address = viewModel.Address,
            PartnerNumber = viewModel.PartnerNumber,
            CroatianPIN = viewModel.CroatianPIN,
            PartnerTypeId = viewModel.PartnerTypeId,
            CreateByUser = viewModel.CreateByUser,
            IsForeign = viewModel.IsForeign,
            ExternalCode = viewModel.ExternalCode,
            Gender = viewModel.Gender,
            SelectedPolicyIds = viewModel.SelectedPolicyIds
        };

        var result = await _partnerService.CreatePartnerAsync(createPartnerDto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);

            viewModel.PartnerTypes = (await _partnerService.GetPartnerTypesAsync()).ToList();
            viewModel.AvailablePolicies = (await _policyService.GetAllPoliciesAsync()).ToList();

            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Partner created successfully.";
        return RedirectToAction("PartnerIndex");

    }
}

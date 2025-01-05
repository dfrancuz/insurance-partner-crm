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
            TotalPolicies = p.PolicyCount,
            TotalPolicyAmount = p.TotalPolicyAmount,
            HasSpecialStatus = p.PolicyCount > 5 || p.TotalPolicyAmount > 5000
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

            ModelState.AddModelError(string.Empty, "There are some issues with your input. Please check all fields and try again.");

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

        TempData["NewPartnerCreated"] = true;
        return RedirectToAction("PartnerIndex");
    }

    // GET /partners/edit/{id}
    [HttpGet("edit/{partnerId}")]
    public async Task<IActionResult> PartnerEdit(int partnerId)
    {
        var partner = await _partnerService.GetPartnerByIdAsync(partnerId);

        if (partner == null)
        {
            return NotFound();
        }

        var viewModel = new EditPartnerViewModel
        {
            PartnerId = partner.PartnerId,
            FirstName = partner.FirstName,
            LastName = partner.LastName,
            Address = partner.Address,
            PartnerNumber = partner.PartnerNumber,
            CroatianPIN = partner.CroatianPIN,
            PartnerTypeId = partner.PartnerTypeId,
            CreateByUser = partner.CreateByUser,
            IsForeign = partner.IsForeign,
            ExternalCode = partner.ExternalCode,
            Gender = partner.Gender,
            PartnerTypes = (await _partnerService.GetPartnerTypesAsync()).ToList(),
            AvailablePolicies = (await _policyService.GetAllPoliciesAsync()).ToList(),
            SelectedPolicyIds = partner.Policies?.Select(p => p.PolicyId).ToList() ?? []
        };

        return View(viewModel);
    }

    // POST /partners/edit/{id}
    [HttpPost("edit/{partnerId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PartnerEdit(int partnerId, EditPartnerViewModel viewModel)
    {
        if (partnerId != viewModel.PartnerId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            viewModel.PartnerTypes = (await _partnerService.GetPartnerTypesAsync()).ToList();
            viewModel.AvailablePolicies = (await _policyService.GetAllPoliciesAsync()).ToList();
            return View(viewModel);
        }

        var updatePartnerDto = new UpdatePartnerDto()
        {
            PartnerId = viewModel.PartnerId,
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

        var result = await _partnerService.UpdatePartnerAsync(updatePartnerDto);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Partner updated successfully.";
            return RedirectToAction("PartnerIndex");
        }

        ModelState.AddModelError(string.Empty, result.Message);
        viewModel.PartnerTypes = (await _partnerService.GetPartnerTypesAsync()).ToList();
        viewModel.AvailablePolicies = (await _policyService.GetAllPoliciesAsync()).ToList();

        return View(viewModel);
    }

    // GET partners/delete/{id}
    [HttpGet("delete/{partnerId}")]
    public async Task<IActionResult> PartnerDelete(int partnerId)
    {
        var partner = await _partnerService.GetPartnerByIdAsync(partnerId);

        if (partner == null)
        {
            return NotFound();
        }

        var viewModel = new DeletePartnerViewModel
        {
            PartnerId = partner.PartnerId,
            FullName = partner.FullName,
            Address = partner.Address,
            PartnerNumber = partner.PartnerNumber,
            CroatianPIN = partner.CroatianPIN,
            PartnerTypeId = partner.PartnerTypeId,
            CreatedAtUtc = partner.CreatedAtUtc,
            CreateByUser = partner.CreateByUser,
            IsForeign = partner.IsForeign,
            ExternalCode = partner.ExternalCode,
            Gender = partner.Gender,
            Policies = partner.Policies?.ToList()
        };

        return View(viewModel);
    }

    // POST /partners/delete/{id}
    [HttpPost("delete/{partnerId}")]
    public async Task<IActionResult> PartnerDeleteConfirmed(int partnerId)
    {
        var result = await _partnerService.DeletePartnerAsync(partnerId);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Partner deleted successfully.";
            return RedirectToAction("PartnerIndex");
        }

        TempData["ErrorMessage"] = result.Message;
        return RedirectToAction("PartnerDelete", new {partnerId});
    }
}

namespace InsurancePartner.Web.Controllers;

using Logic.DTOs;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.PolicyViewModels;

[Route("policies")]
public class PoliciesController : Controller
{
    private readonly IPolicyService _policyService;

    public PoliciesController(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    // GET /policies
    [HttpGet]
    public async Task<IActionResult> PolicyIndex()
    {
        var policies = await _policyService.GetAllPoliciesAsync();

        var viewModels = policies.Select(p => new PolicyListViewModel
        {
            PolicyId = p.PolicyId,
            PolicyNumber = p.PolicyNumber,
            Amount = p.Amount,
            CreatedAtUtc = p.CreatedAtUtc
        }).OrderByDescending(p => p.CreatedAtUtc);

        return View(viewModels);
    }

    // GET policies/create
    [HttpGet("create")]
    public IActionResult PolicyCreate()
    {
        var viewModel = new CreatePolicyViewModel();

        return View(viewModel);
    }

    // POST policies/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PolicyCreate(CreatePolicyViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "There are some issues with your input. please check all fields and try again.");
            return View(viewModel);
        }

        var createPolicyDto = new CreatePolicyDto
        {
            PolicyNumber = viewModel.PolicyNumber,
            Amount = viewModel.Amount
        };

        var result = await _policyService.CreatePolicyAsync(createPolicyDto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(viewModel);
        }

        TempData["NewPolicyCreated"] = true;
        return RedirectToAction("PolicyIndex");
    }

    // GET /policies/edit/{id}
    [HttpGet("edit/{policyId}")]
    public async Task<IActionResult> PolicyEdit(int policyId)
    {
        var policy = await _policyService.GetPolicyByIdAsync(policyId);

        if (policy == null)
        {
            return NotFound();
        }

        var viewModel = new EditPolicyViewModel
        {
            PolicyId = policy.PolicyId,
            PolicyNumber = policy.PolicyNumber,
            Amount = policy.Amount
        };

        return View(viewModel);
    }

    // POST /policies/edit/{id}
    [HttpPost("edit/{policyId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PolicyEdit(int policyId, EditPolicyViewModel viewModel)
    {
        if (policyId != viewModel.PolicyId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Something went wrong with your request.");
            return View(viewModel);
        }

        var updatePolicyDto = new UpdatePolicyDto
        {
            PolicyId = viewModel.PolicyId,
            PolicyNumber = viewModel.PolicyNumber,
            Amount = viewModel.Amount
        };

        var result = await _policyService.UpdatePolicyAsync(updatePolicyDto);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Policy updated successfully";
            return RedirectToAction("PolicyIndex");
        }

        ModelState.AddModelError(string.Empty, result.Message);

        return View(viewModel);
    }
}

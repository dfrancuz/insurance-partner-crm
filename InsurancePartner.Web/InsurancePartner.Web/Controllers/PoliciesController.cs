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
}

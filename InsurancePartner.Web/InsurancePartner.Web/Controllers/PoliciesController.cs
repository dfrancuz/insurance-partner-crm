namespace InsurancePartner.Web.Controllers;

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
}

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewWebsite.Models;
using NewWebsite.Services;

namespace NewWebsite.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    public async Task<IActionResult> SignIn()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        if (ModelState.IsValid)
        {
            /// business logic
            try
            {
                User user = await _accountService.AuthenticateAsync(request);

                return RedirectToAction("Index", "Home");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                ModelState.AddModelError("Username", ex.Message);
            }
        }

        return View(request);
    }

    // GET: News/Detail/5
    public async Task<IActionResult> SignUp()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        if (ModelState.IsValid)
        {
            if (request.IsAgreedTerms != true)
            {
                ModelState.AddModelError(nameof(request.IsAgreedTerms), "You must agree to our terms to continue with registration");
            }
        }

        if (ModelState.IsValid)
        {
            /// business logic
            try
            {
                User user = await _accountService.RegisterNewUser(request);

                return RedirectToAction(nameof(SignIn));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                ModelState.AddModelError("Username", ex.Message);
            }
        }

        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> SignOut()
    {
        await _accountService.SignOutAsync();

        return RedirectToAction(nameof(SignIn));
    }

    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }
}

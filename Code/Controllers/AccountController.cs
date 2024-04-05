using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Bootstrapping;
using Web.Models;

namespace Web.Controllers;

[AllowAnonymous]
public class AccountController(
    ILogger<RegisterModel> logger,
    CognitoUserPool pool,
    SignInManager<CognitoUser> signInManager,
    UserManager<CognitoUser> userManager) : Controller
{
    private readonly ILogger<RegisterModel> _logger = logger;
    private readonly CognitoUserPool _pool = pool;
    private readonly SignInManager<CognitoUser> _signInManager = signInManager;
    private readonly CognitoUserManager<CognitoUser>? _userManager = userManager as CognitoUserManager<CognitoUser>;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        CognitoUser user = _pool.GetUser(registerModel.Username);
        user.Attributes.Add(CognitoAttribute.Email.AttributeName, registerModel.EmailAddress);

        if (_userManager != null)
        {
            IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                _logger.LogInformationMessage("User created a new account with password.");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(signInModel.Username, signInModel.Password, true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            _logger.LogInformationMessage("User logged in.");
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformationMessage("User logged out.");

        return RedirectToAction("Index", "Home");
    }
}
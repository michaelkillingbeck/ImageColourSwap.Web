using Amazon.AspNetCore.Identity.Cognito;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ILogger<RegisterModel> _logger;
    private readonly CognitoUserPool _pool;
    private readonly SignInManager<CognitoUser> _signInManager;
    private readonly CognitoUserManager<CognitoUser>? _userManager;

    public AccountController(
        ILogger<RegisterModel> logger,
        CognitoUserPool pool,
        SignInManager<CognitoUser> signInManager,
        UserManager<CognitoUser> userManager)
    {
        _logger = logger;
        _pool = pool;
        _signInManager = signInManager;
        _userManager = userManager as CognitoUserManager<CognitoUser>;
    }

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
        var user = _pool.GetUser(registerModel.Username);
        user.Attributes.Add(CognitoAttribute.Email.AttributeName, registerModel.EmailAddress);

        if(_userManager != null)
        {
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        var result = await _signInManager.PasswordSignInAsync(signInModel.Username, signInModel.Password, true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");

        return RedirectToAction("Index", "Home");
    }
}
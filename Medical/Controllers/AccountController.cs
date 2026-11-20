using System.Globalization;
using Medical.Models;
using Medical.Services;
using Medical.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly Translator _t;

    public AccountController(UserManager<ApplicationUser> users,
        SignInManager<ApplicationUser> signIn, Translator t)
    {
        _users = users; _signIn = signIn; _t = t;
    }

    [HttpGet] public IActionResult Register() => View(new RegisterVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = new ApplicationUser
        {
            UserName = vm.Phone,
            PhoneNumber = vm.Phone,
            Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email.Trim(),
            FullName = vm.FullName.Trim(),
            PreferredLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
        };

        var result = await _users.CreateAsync(user, vm.Password);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(vm);
        }

        await _users.AddToRoleAsync(user, "Patient");
        await _signIn.SignInAsync(user, isPersistent: true);   // role add-er por sign-in
        return RedirectToAction("Age", "Onboarding");
    }

    [HttpGet] public IActionResult Login() => View(new LoginVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var id = vm.Identifier.Trim();
        var user = id.Contains('@') ? await _users.FindByEmailAsync(id)
                                    : await _users.FindByNameAsync(id);

        if (user is not null)
        {
            var res = await _signIn.PasswordSignInAsync(user, vm.Password, vm.RememberMe, lockoutOnFailure: true);
            if (res.Succeeded)
            {
                if (await _users.IsInRoleAsync(user, "Doctor")) return Redirect("/doctor");
                if (!user.OnboardingCompleted) return RedirectToAction("Age", "Onboarding");
                return Redirect("/patient");
            }
        }

        ModelState.AddModelError("", _t["InvalidLogin"]);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => StatusCode(403);
}
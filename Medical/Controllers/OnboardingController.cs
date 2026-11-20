using Medical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers;

[Authorize]
public class OnboardingController : Controller
{
    private readonly UserManager<ApplicationUser> _users;
    public OnboardingController(UserManager<ApplicationUser> users) => _users = users;

    [HttpGet]
    public async Task<IActionResult> Age()
    {
        var user = await _users.GetUserAsync(User);
        if (user is { OnboardingCompleted: true }) return Redirect("/patient");   // ek-bar-i jiggesh
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Age(AgeGroup ageGroup)
    {
        if (ageGroup == AgeGroup.NotSet) return View();

        var user = await _users.GetUserAsync(User);
        if (user is null) return Challenge();

        user.AgeGroup = ageGroup;
        user.OnboardingCompleted = true;
        await _users.UpdateAsync(user);

        return Redirect("/patient");
    }
}
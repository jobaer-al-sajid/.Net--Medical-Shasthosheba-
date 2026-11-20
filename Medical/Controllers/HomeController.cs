using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
            return Redirect(User.IsInRole("Doctor") ? "/doctor" : "/patient");
        return View();
    }

    public IActionResult Error() => Content("Something went wrong. Please try again.");
}
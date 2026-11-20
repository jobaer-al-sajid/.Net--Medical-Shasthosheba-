using Medical.Data;
using Medical.Models;
using Medical.Services;
using Medical.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical.Controllers;

[Authorize(Roles = "Patient")]
[Route("patient")]
public class PatientController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _users;
    private readonly GuidanceService _guidance;
    private readonly Translator _t;

    public PatientController(AppDbContext db, UserManager<ApplicationUser> users,
        GuidanceService guidance, Translator t)
    {
        _db = db; _users = users; _guidance = guidance; _t = t;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var user = await _users.GetUserAsync(User);
        if (user is null) return Challenge();
        if (!user.OnboardingCompleted) return RedirectToAction("Age", "Onboarding");

        var doctors = await _db.DoctorProfiles
            .Include(d => d.User).Include(d => d.Specialty)
            .Where(d => d.IsVerified).ToListAsync();

        var next = await _db.Appointments
            .Where(a => a.PatientId == user.Id
                        && a.Status != AppointmentStatus.Cancelled
                        && a.ScheduledAtUtc > DateTime.UtcNow)
            .OrderBy(a => a.ScheduledAtUtc)
            .FirstOrDefaultAsync();

        var vm = new PatientDashboardVm
        {
            FullName = user.FullName,
            Guidance = _guidance.GetTodayGuidance(user.AgeGroup, _t.IsBangla),
            ShowHomeNurse = user.AgeGroup == AgeGroup.Age50Plus,
            NextVisitText = next?.ScheduledAtUtc is { } scheduled
                ? BdTime.FromUtc(scheduled).ToString("ddd, h:mm tt")
                : _t["NoVisit"],
            TopDoctors = doctors.OrderByDescending(DoctorRanking.Score).Take(3).ToList()
        };
        return View(vm);
    }
}
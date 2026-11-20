using System.Security.Claims;
using Medical.Data;
using Medical.Models;
using Medical.Services;
using Medical.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical.Controllers;

[Authorize(Roles = "Doctor")]
[Route("doctor")]
public class DoctorPortalController : Controller
{
    private readonly AppDbContext _db;
    private readonly Translator _t;

    public DoctorPortalController(AppDbContext db, Translator t) { _db = db; _t = t; }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var profile = await _db.DoctorProfiles.Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == userId);
        if (profile is null) return Content("Doctor profile not found. Contact admin.");

        var now = DateTime.UtcNow;
        // Bangladesh-er aajker shuru (UTC-te)
        var todayStartUtc = BdTime.FromUtc(now).Date.AddHours(-6);

        var waiting = await _db.Appointments.Include(a => a.Patient)
            .Where(a => a.DoctorProfileId == profile.Id && a.Status == AppointmentStatus.Waiting)
            .OrderBy(a => a.CreatedAtUtc)               // sobcheye beshi opekkha age
            .ToListAsync();

        var visitsToday = await _db.Appointments
            .CountAsync(a => a.DoctorProfileId == profile.Id && a.CreatedAtUtc >= todayStartUtc);

        return View(new DoctorDashboardVm
        {
            DoctorName = profile.User.FullName,
            VisitsToday = visitsToday,
            AvgReplyMinutes = profile.AvgReplyMinutes,
            Rating = profile.AvgRating,
            Queue = waiting.Select(a => new QueueItemVm
            {
                PatientName = a.Patient.FullName,
                AgeLabel = _t.AgeGroupLabel(a.Patient.AgeGroup),
                Reason = a.Reason,
                WaitingMinutes = (int)(now - a.CreatedAtUtc).TotalMinutes
            }).ToList()
        });
    }
}
using Medical.Data;
using Medical.Models;
using Medical.Services;
using Medical.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical.Controllers;

[Route("doctors")]
public class DoctorsController : Controller
{
    private readonly AppDbContext _db;
    public DoctorsController(AppDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index() =>
        View(await _db.Specialties.OrderBy(s => s.Id).ToListAsync());

    [HttpGet("{slug}")]
    public async Task<IActionResult> Specialty(string slug, string sort = "top")
    {
        var spec = await _db.Specialties.FirstOrDefaultAsync(s => s.Slug == slug);
        if (spec is null) return NotFound();

        var list = await _db.DoctorProfiles
            .Include(d => d.User)
            .Where(d => d.SpecialtyId == spec.Id && d.IsVerified)
            .ToListAsync();

        var topPickId = list.OrderByDescending(DoctorRanking.Score).FirstOrDefault()?.Id ?? 0;

        IEnumerable<DoctorProfile> sorted = sort switch
        {
            "fast" => list.OrderBy(d => d.AvgReplyMinutes),
            "exp" => list.OrderByDescending(d => d.YearsExperience),
            "fee" => list.OrderBy(d => d.Fee),
            _ => list.OrderByDescending(DoctorRanking.Score)
        };

        // Top Pick shobsomoy prothome (OrderBy stable, baki order thik thake)
        sorted = sorted.OrderByDescending(d => d.Id == topPickId);

        return View(new DoctorListVm
        {
            Specialty = spec,
            Sort = sort,
            TopPickId = topPickId,
            Doctors = sorted.ToList()
        });
    }
}
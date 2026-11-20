using System.Security.Claims;
using Medical.Data;
using Medical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical.Controllers;

[Authorize]
[Route("help")]
public class HelpController : Controller
{
    private readonly AppDbContext _db;
    public HelpController(AppDbContext db) => _db = db;

    // /help -> form + nijer request-er list
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var list = await _db.Helps.AsNoTracking()
            .Where(h => h.UserId == uid)
            .OrderByDescending(h => h.CreatedAtUtc)
            .ToListAsync();
        return View(list);
    }

    // form submit
    [HttpPost("send")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
            return RedirectToAction(nameof(Index));

        _db.Helps.Add(new Help
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Title = title.Trim(),
            Message = message.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        TempData["ok"] = "1";
        return RedirectToAction(nameof(Index));
    }
}
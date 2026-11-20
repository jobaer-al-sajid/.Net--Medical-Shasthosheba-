using Microsoft.AspNetCore.Identity;

namespace Medical.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = "";
    public AgeGroup AgeGroup { get; set; } = AgeGroup.NotSet;
    public bool OnboardingCompleted { get; set; }
    public string PreferredLanguage { get; set; } = "bn";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
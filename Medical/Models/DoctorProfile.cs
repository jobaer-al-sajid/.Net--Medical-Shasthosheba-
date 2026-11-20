namespace Medical.Models;

public class DoctorProfile
{
    public int Id { get; set; }

    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;

    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;

    public string BmdcNumber { get; set; } = "";
    public bool IsVerified { get; set; }

    public int YearsExperience { get; set; }
    public decimal Fee { get; set; }

    public double AvgRating { get; set; }
    public int ReviewCount { get; set; }
    public int AvgReplyMinutes { get; set; }
    public double FollowUpScore { get; set; }
}
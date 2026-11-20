using Medical.Models;

namespace Medical.Services;

public static class DoctorRanking
{
    public static double Score(DoctorProfile d)
    {
        var rating = Math.Clamp(d.AvgRating / 5.0, 0, 1);
        var reply = 1.0 - Math.Min(d.AvgReplyMinutes, 60) / 60.0;   // joto druto, toto beshi
        var exp = Math.Min(d.YearsExperience, 20) / 20.0;
        var follow = Math.Clamp(d.FollowUpScore, 0, 1);

        return 0.35 * rating + 0.30 * reply + 0.20 * exp + 0.15 * follow;
    }
}
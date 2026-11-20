namespace Medical.Services;

public static class BdTime
{
    // Bangladesh = UTC+6
    public static DateTime FromUtc(DateTime utc) => utc.AddHours(6);
}
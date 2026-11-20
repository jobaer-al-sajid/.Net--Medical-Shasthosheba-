namespace Medical.Models;

public enum AgeGroup
{
    NotSet = 0,
    Under18 = 1,
    Age18To39 = 2,
    Age40To49 = 3,
    Age50Plus = 4
}

public enum AppointmentStatus
{
    Waiting = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3
}
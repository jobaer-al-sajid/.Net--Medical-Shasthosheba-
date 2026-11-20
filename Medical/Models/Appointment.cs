namespace Medical.Models;

public class Appointment
{
    public int Id { get; set; }

    public string PatientId { get; set; } = "";
    public ApplicationUser Patient { get; set; } = null!;

    public int DoctorProfileId { get; set; }
    public DoctorProfile DoctorProfile { get; set; } = null!;

    public string Reason { get; set; } = "";
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Waiting;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledAtUtc { get; set; }
}
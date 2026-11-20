using System.ComponentModel.DataAnnotations;
using Medical.Models;

namespace Medical.ViewModels;

public class RegisterVm
{
    [Required, StringLength(80)]
    public string FullName { get; set; } = "";

    [Required]
    [RegularExpression(@"^01[3-9]\d{8}$", ErrorMessage = "Enter a valid mobile number like 01712345678")]
    public string Phone { get; set; } = "";

    [EmailAddress]
    public string? Email { get; set; }

    [Required, DataType(DataType.Password), MinLength(6)]
    public string Password { get; set; } = "";
}

public class LoginVm
{
    [Required]
    public string Identifier { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; } = true;
}

public class PatientDashboardVm
{
    public string FullName { get; set; } = "";
    public string Guidance { get; set; } = "";
    public string NextVisitText { get; set; } = "";
    public bool ShowHomeNurse { get; set; }
    public List<DoctorProfile> TopDoctors { get; set; } = new();
}

public class DoctorListVm
{
    public Specialty Specialty { get; set; } = null!;
    public string Sort { get; set; } = "top";
    public int TopPickId { get; set; }
    public List<DoctorProfile> Doctors { get; set; } = new();
}

public class QueueItemVm
{
    public string PatientName { get; set; } = "";
    public string AgeLabel { get; set; } = "";
    public string Reason { get; set; } = "";
    public int WaitingMinutes { get; set; }
}

public class DoctorDashboardVm
{
    public string DoctorName { get; set; } = "";
    public int VisitsToday { get; set; }
    public int AvgReplyMinutes { get; set; }
    public double Rating { get; set; }
    public List<QueueItemVm> Queue { get; set; } = new();
}
using Medical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Medical.Data;

public static class DbSeeder
{
    private record DemoDoctor(string Name, string Phone, string Slug, string Bmdc,
        int Years, decimal Fee, double Rating, int Reviews, int Reply, double Follow);

    public static async Task SeedAsync(IServiceProvider sp, bool seedDemo)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await db.Database.MigrateAsync();

        foreach (var r in new[] { "Patient", "Doctor", "Nurse", "Admin" })
            if (!await roles.RoleExistsAsync(r))
                await roles.CreateAsync(new IdentityRole(r));

        if (!await db.Specialties.AnyAsync())
        {
            db.Specialties.AddRange(
                new Specialty { Slug = "general-medicine", NameEn = "General Medicine", NameBn = "জেনারেল মেডিসিন", Icon = "🩺" },
                new Specialty { Slug = "orthopedics", NameEn = "Orthopedics", NameBn = "অর্থোপেডিক্স", Icon = "🦴" },
                new Specialty { Slug = "neurology", NameEn = "Neurology", NameBn = "নিউরোলজি", Icon = "🧠" },
                new Specialty { Slug = "gynecology", NameEn = "Gynecology", NameBn = "গাইনোকোলজি", Icon = "🤰" },
                new Specialty { Slug = "ophthalmology", NameEn = "Ophthalmology", NameBn = "চক্ষু", Icon = "👁️" },
                new Specialty { Slug = "cardiology", NameEn = "Cardiology", NameBn = "কার্ডিওলজি", Icon = "❤️" },
                new Specialty { Slug = "pediatrics", NameEn = "Pediatrics", NameBn = "শিশু রোগ", Icon = "👶" },
                new Specialty { Slug = "ent", NameEn = "ENT", NameBn = "নাক-কান-গলা", Icon = "👂" },
                new Specialty { Slug = "dermatology", NameEn = "Dermatology", NameBn = "চর্মরোগ", Icon = "🖐️" },
                new Specialty { Slug = "psychiatry", NameEn = "Psychiatry", NameBn = "মানসিক স্বাস্থ্য", Icon = "💬" },
                new Specialty { Slug = "dentistry", NameEn = "Dentistry", NameBn = "দন্ত", Icon = "🦷" },
                new Specialty { Slug = "urology", NameEn = "Urology", NameBn = "ইউরোলজি", Icon = "🧪" });
            await db.SaveChangesAsync();
        }

        if (!seedDemo || await db.DoctorProfiles.AnyAsync()) return;

        var spec = await db.Specialties.ToDictionaryAsync(s => s.Slug);

        var demoDoctors = new[]
        {
            new DemoDoctor("Dr. Anwar Karim",    "01700000001", "neurology",        "A-10001", 15, 1200, 4.9, 320, 10, 0.95),
            new DemoDoctor("Dr. Sabrina Rahman", "01700000002", "neurology",        "A-10002", 11, 1000, 4.8, 210, 20, 0.85),
            new DemoDoctor("Dr. Mahmud Hasan",   "01700000003", "neurology",        "A-10003",  8,  800, 4.6, 140, 45, 0.70),
            new DemoDoctor("Dr. Nusrat Jahan",   "01700000004", "general-medicine", "A-10004", 12,  700, 4.8, 260, 15, 0.90),
            new DemoDoctor("Dr. Imran Hossain",  "01700000005", "ophthalmology",    "A-10005", 10,  900, 4.8, 180, 18, 0.88),
        };

        DoctorProfile? first = null;
        foreach (var d in demoDoctors)
        {
            var u = await CreateUser(users, d.Name, d.Phone, "Doctor", AgeGroup.Age40To49);
            var profile = new DoctorProfile
            {
                UserId = u.Id,
                SpecialtyId = spec[d.Slug].Id,
                BmdcNumber = d.Bmdc,
                IsVerified = true,
                YearsExperience = d.Years,
                Fee = d.Fee,
                AvgRating = d.Rating,
                ReviewCount = d.Reviews,
                AvgReplyMinutes = d.Reply,
                FollowUpScore = d.Follow
            };
            db.DoctorProfiles.Add(profile);
            first ??= profile;
        }
        await db.SaveChangesAsync();

        var demoPatients = new[]
        {
            ("Rahim Uddin",  "01800000001", AgeGroup.Age50Plus, "Headache, follow-up", 25),
            ("Farida Begum", "01800000002", AgeGroup.Age18To39, "New consultation",    12),
            ("Tanvir Ahmed", "01800000003", AgeGroup.Age18To39, "Report review",        5),
        };
        foreach (var (name, phone, age, reason, waited) in demoPatients)
        {
            var p = await CreateUser(users, name, phone, "Patient", age);
            db.Appointments.Add(new Appointment
            {
                PatientId = p.Id,
                DoctorProfileId = first!.Id,
                Reason = reason,
                Status = AppointmentStatus.Waiting,
                CreatedAtUtc = DateTime.UtcNow.AddMinutes(-waited)
            });
        }
        await db.SaveChangesAsync();
    }

    private static async Task<ApplicationUser> CreateUser(
        UserManager<ApplicationUser> um, string name, string phone, string role, AgeGroup age)
    {
        var u = new ApplicationUser
        {
            UserName = phone,
            PhoneNumber = phone,
            PhoneNumberConfirmed = true,
            FullName = name,
            AgeGroup = age,
            OnboardingCompleted = true
        };
        var r = await um.CreateAsync(u, "Demo1234");
        if (!r.Succeeded) throw new Exception(string.Join(", ", r.Errors.Select(e => e.Description)));
        await um.AddToRoleAsync(u, role);
        return u;
    }
}
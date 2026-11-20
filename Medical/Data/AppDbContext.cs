using Medical.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Medical.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<DoctorProfile> DoctorProfiles => Set<DoctorProfile>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Help> Helps => Set<Help>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<Specialty>().HasIndex(s => s.Slug).IsUnique();
        b.Entity<DoctorProfile>().Property(d => d.Fee).HasPrecision(10, 2);

        // SQL Server "multiple cascade paths" error theke bachar jonno
        b.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
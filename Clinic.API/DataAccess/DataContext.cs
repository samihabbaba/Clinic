using Clinic.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic.API.DataAccess
{
    public class DataContext : IdentityDbContext<SystemUser>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<SystemUser>  SystemUsers { get; set; }
        public DbSet<Value> Values { get; set; }
        public DbSet<Appointments> Appointment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointments>()
                .HasKey(k => new {k.PatientId, k.DoctorId});

            builder.Entity<Appointments>()
                .HasOne(u => u.Patient)
                .WithMany(u => u.Doctors)
                .HasForeignKey(u => u.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
                

            builder.Entity<Appointments>()
                .HasOne(u => u.Doctor)
                .WithMany(u => u.Patients)
                .HasForeignKey(u => u.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
                
        }
    }
}
using Appointments.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppointmentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceCategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

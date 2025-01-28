using Microsoft.EntityFrameworkCore;
using Documents.Domain.Entities;
using Documents.Infrastructure.EntityConfigurations;

namespace Documents.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServiceCategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationConfiguration());
            modelBuilder.ApplyConfiguration(new AccountEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ResultEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

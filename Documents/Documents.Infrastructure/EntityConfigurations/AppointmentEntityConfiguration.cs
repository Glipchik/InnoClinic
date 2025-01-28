using Documents.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.EntityConfigurations
{
    public class AppointmentEntityConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Service)
               .WithMany()
               .HasForeignKey(a => a.ServiceId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

            builder.HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(a => a.Result)
                .WithOne()
                .HasForeignKey<Result>(r => r.AppointmentId);

            builder.HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(a => a.Date)
                .IsRequired();

            builder.Property(a => a.Time)
                .IsRequired();
        }
    }
}

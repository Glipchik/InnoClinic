using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.EntityConfigurations
{
    public class DoctorEntityConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(d => d.MiddleName)
                .HasMaxLength(63);

            builder.Property(d => d.CareerStartYear)
                .IsRequired();

            builder.Property(d => d.DateOfBirth)
                .IsRequired();

            builder.Property(d => d.Status)
                .IsRequired();

            builder.HasOne(d => d.Specialization)
                .WithMany()
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}

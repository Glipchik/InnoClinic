using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.EntityConfigurations
{
    public class PatientEntityConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(p => p.MiddleName)
                .HasMaxLength(63);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();
        }
    }
}

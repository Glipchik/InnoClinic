using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.EntityConfigurations
{
    public class SpecializationEntityConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.Property(s => s.SpecializationName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(s => s.IsActive)
                .IsRequired();
        }
    }
}

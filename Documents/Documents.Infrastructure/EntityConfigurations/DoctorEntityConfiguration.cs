using Documents.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.EntityConfigurations
{
    public class DoctorEntityConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(d => d.MiddleName)
                .HasMaxLength(63);

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

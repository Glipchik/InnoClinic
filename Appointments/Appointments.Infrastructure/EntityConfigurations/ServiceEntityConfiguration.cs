using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.EntityConfigurations
{
    public class ServiceEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(s => s.ServiceName)
               .IsRequired()
               .HasMaxLength(63);

            builder.HasOne(s => s.ServiceCategory)
                .WithMany()
                .HasForeignKey(d => d.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(s => s.Specialization)
                .WithMany()
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(s => s.Price)
                .HasAnnotation("Range", new[] { 0.0, double.MaxValue })
                .IsRequired();
        }
    }
}

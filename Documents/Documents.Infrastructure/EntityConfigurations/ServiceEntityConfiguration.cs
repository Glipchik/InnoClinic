using Documents.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.EntityConfigurations
{
    public class ServiceEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

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

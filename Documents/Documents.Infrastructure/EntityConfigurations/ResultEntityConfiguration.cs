using Documents.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.EntityConfigurations
{
    public class ResultEntityConfiguration : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Appointment)
                .WithOne(a => a.Result)
                .HasForeignKey<Result>(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(r => r.Complaints)
               .IsRequired()
               .HasMaxLength(500);

            builder.Property(r => r.Recomendations)
               .IsRequired()
               .HasMaxLength(500);

            builder.Property(r => r.Conclusion)
               .IsRequired()
               .HasMaxLength(500);
        }
    }
}

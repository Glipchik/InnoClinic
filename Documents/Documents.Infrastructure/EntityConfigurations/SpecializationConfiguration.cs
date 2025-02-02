using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Documents.Domain.Entities;

namespace Documents.Infrastructure.EntityConfigurations
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SpecializationName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Documents.Domain.Entities;

namespace Documents.Infrastructure.EntityConfigurations
{
    public class ServiceCategoryEntityConfiguration : IEntityTypeConfiguration<ServiceCategory>
    {
        public void Configure(EntityTypeBuilder<ServiceCategory> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Infrastructure.EntityConfigurations
{
    public class ServiceCategoryEntityConfiguration : IEntityTypeConfiguration<ServiceCategory>
    {
        public void Configure(EntityTypeBuilder<ServiceCategory> builder)
        {
            builder.Property(sc => sc.CategoryName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(sc => sc.TimeSlotSize)
                .IsRequired();
        }
    }
}

using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Infrastructure.EntityConfigurations
{
    public class PatientEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
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

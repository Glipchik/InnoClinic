using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Infrastructure.EntityConfigurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
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

            builder.Property(d => d.CareerStartYear)
                .IsRequired();

            builder.Property(d => d.DateOfBirth)
                .IsRequired();

            builder.Property(d => d.Status)
                .IsRequired();

            builder.HasOne(d => d.Account)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Specialization)
                .WithMany()
                .HasForeignKey(d => d.SpecializationId);

            builder.HasOne(d => d.Office)
                .WithMany()
                .HasForeignKey(d => d.OfficeId);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Infrastructure.EntityConfigurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(p => p.MiddleName)
                .HasMaxLength(63);

            builder.Property(p => p.IsLinkedToAccount)
                .IsRequired();

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.HasOne(p => p.Account)
                .WithOne()
                .HasForeignKey<Patient>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

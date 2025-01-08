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
    public class ReceptionistConfiguration : IEntityTypeConfiguration<Receptionist>
    {
        public void Configure(EntityTypeBuilder<Receptionist> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.FirstName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(r => r.LastName)
                .IsRequired()
                .HasMaxLength(63);

            builder.Property(r => r.MiddleName)
                .HasMaxLength(63);

            builder.HasOne(r => r.Account)
                .WithOne()
                .HasForeignKey<Patient>(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Office)
                .WithMany()
                .HasForeignKey(r => r.OfficeId);
        }
    }
}

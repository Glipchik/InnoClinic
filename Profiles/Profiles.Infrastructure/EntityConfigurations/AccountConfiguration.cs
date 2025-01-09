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
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasIndex(a => a.Email).IsUnique();

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.PhotoFileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.CreatedBy)
                .IsRequired();

            builder.Property(a => a.UpdatedBy)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            builder.Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}

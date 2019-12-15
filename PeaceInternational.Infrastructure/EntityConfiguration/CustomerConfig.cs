using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class CustomerConfig : BaseEntityConfig<Customer>, IEntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.HasAlternateKey(p => p.FileCodeNo);

            builder.Property(p => p.FileCodeNo)     
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.TourName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Country)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Agent)
                .HasMaxLength(255);

            builder.Property(p => p.AgentStaff)
                .HasMaxLength(255);

            builder.Property(p => p.GuideName)
                .HasMaxLength(255);

            builder.HasOne(p => p.FiscalYear)
                .WithMany(e => e.Customers)
                .HasForeignKey(p => p.FiscalYearId);
        }
    }
}

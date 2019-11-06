using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class InvoiceConfig : BaseEntityConfig<Invoice>, IEntityTypeConfiguration<Invoice>
    {
        public override void Configure(EntityTypeBuilder<Invoice> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.InvoiceNo)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.FileCodeNo)
                .IsRequired();

            builder.Property(p => p.ReferenceNo)
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.Dr)
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.AgentName)
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.ClientName)
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.PAX)
               .IsRequired();

            builder.Property(p => p.Currency)
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.Guide)
               .HasMaxLength(255);


            builder.Property(p => p.Vehicle)
               .HasMaxLength(255);
        }
    }
}

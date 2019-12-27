using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class TourcostConfig : BaseEntityConfig<Tourcost>, IEntityTypeConfiguration<Tourcost>
    {
        public override void Configure(EntityTypeBuilder<Tourcost> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Category1)
                .HasMaxLength(50);

            builder.Property(p => p.Category2)
                .HasMaxLength(50);

            builder.Property(p => p.Category3)
                .HasMaxLength(50);

            builder.Property(p => p.Category4)
                .HasMaxLength(50);

            builder.Property(p => p.Category5)
                .HasMaxLength(50);            

            builder.Property(p => p.GrossAmountAccomodation)
               .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.GrossAmountTransportation)
               .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.NetAmountAccomodation)
               .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.NetAmountTransportation)
              .HasColumnType("Decimal(10,2)");

            builder.HasOne(p => p.Guide)
                .WithMany(e => e.Tourcost)
                .HasForeignKey(p => p.GuideId);
        }
    }
}

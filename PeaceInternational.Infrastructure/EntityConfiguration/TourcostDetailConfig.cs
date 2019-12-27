using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class TourcostDetailConfig : BaseEntityConfig<TourcostDetail>, IEntityTypeConfiguration<TourcostDetail>
    {
        public override void Configure(EntityTypeBuilder<TourcostDetail> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Day)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasOne(p => p.Tourcost)
                .WithMany(e => e.TourcostDetail)
                .HasForeignKey(p => p.TourcostId);

            builder.HasOne(p => p.Sector1)
                .WithMany(e => e.TourcostDetail1)
                .HasForeignKey(p => p.Sector1Id);

            builder.HasOne(p => p.Sector2)
             .WithMany(e => e.TourcostDetail2)
             .HasForeignKey(p => p.Sector2Id);


            builder.Property(p => p.Category1Cost)
               .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.Category2Cost)
             .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.Category3Cost)
             .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.Category4Cost)
             .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.Category5Cost)
             .HasColumnType("Decimal(10,2)");

            builder.HasOne(p => p.HotelA)
            .WithMany(e => e.TourcostDetailA)
            .HasForeignKey(p => p.HotelAId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(p => p.HotelB)
           .WithMany(e => e.TourcostDetailB)
           .HasForeignKey(p => p.HotelBId)
           .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(p => p.HotelC)
           .WithMany(e => e.TourcostDetailC)
           .HasForeignKey(p => p.HotelCId)
           .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}

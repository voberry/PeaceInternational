using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class HotelConfig : BaseEntityConfig<Hotel>, IEntityTypeConfiguration<Hotel>
    {
        public override void Configure(EntityTypeBuilder<Hotel> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                 .IsRequired()
                 .HasMaxLength(256);

            builder.Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(p => p.PhoneNo)
                .IsRequired();

            builder.Property(p => p.Code)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(p => p.Category)
               .IsRequired()
               .HasColumnType("char(1)");

            builder.HasMany(p => p.TourcostDetailA)
                .WithOne(e => e.HotelA)
                .HasForeignKey(e => e.HotelAId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(p => p.TourcostDetailB)
              .WithOne(e => e.HotelB)
              .HasForeignKey(e => e.HotelBId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(p => p.TourcostDetailC)
              .WithOne(e => e.HotelC)
              .HasForeignKey(e => e.HotelCId)
              .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}

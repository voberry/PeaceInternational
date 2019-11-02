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
        }
    }
}

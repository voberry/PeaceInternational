using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class TransportConfig : BaseEntityConfig<Transport>, IEntityTypeConfiguration<Transport>
    {
        public override void Configure(EntityTypeBuilder<Transport> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasData(new Transport()
            {
                Id = 1,
                Name = "T1",
                MinPAX = 3,
                MaxPAX = 5,
                CreatedBy = "superadmin"
            });

            builder.HasData(new Transport()
            {
                Id = 2,
                Name = "T2",
                MinPAX = 6,
                MaxPAX = 9,
                CreatedBy = "superadmin"
            });

            builder.HasData(new Transport()
            {
                Id = 3,
                Name = "T3",
                MinPAX = 10,
                MaxPAX = 14,
                CreatedBy = "superadmin"
            });

            builder.HasData(new Transport()
            {
                Id = 4,
                Name = "T4",
                MinPAX = 15,
                MaxPAX = 19,
                CreatedBy = "superadmin"
            });

            builder.HasData(new Transport()
            {
                Id = 5,
                Name = "T5",
                MinPAX = 20,
                MaxPAX = 24,
                CreatedBy = "superadmin"
            });

            builder.HasData(new Transport()
            {
                Id = 6,
                Name = "T6",
                MinPAX = 25,
                MaxPAX = 25,
                CreatedBy = "superadmin"
            });
        }
    }
}

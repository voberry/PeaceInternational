using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class GuideEntityConfig : BaseEntityConfig<Guide>, IEntityTypeConfiguration<Guide>
    {
        public override void Configure(EntityTypeBuilder<Guide> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.FullDayRate)
               .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.HalfDayRate)
               .HasColumnType("Decimal(10,2)");

        }
    }
}

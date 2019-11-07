using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeaceInternational.Infrastructure.EntityConfiguration;

namespace PeaceInternational.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new HotelConfig());
            builder.ApplyConfiguration(new InvoiceConfig());
            builder.ApplyConfiguration(new InvoiceDetailConfig());
            builder.ApplyConfiguration(new HotelReceiptConfig());

        }
    }
}

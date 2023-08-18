using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Persistence.Entities;

namespace PaymentGateway.Data.Persistence
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext() { }
        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options) { }
        protected PaymentGatewayDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder != null && !optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("PaymentGatewayDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
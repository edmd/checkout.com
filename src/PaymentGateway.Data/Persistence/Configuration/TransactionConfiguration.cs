using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction = PaymentGateway.Data.Persistence.Entities.Transaction;

namespace ChambersCentral.Submissions.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.HasKey(x => x.TransactionId);

            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.CardHolderName).IsRequired();
            builder.Property(x => x.CardNumber).IsRequired();
            builder.Property(x => x.CurrencyCode).IsRequired();
            builder.Property(x => x.Cvv2).IsRequired();
            builder.Property(x => x.MerchantId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ValidFrom);
            builder.Property(x => x.ValidTo).IsRequired();
        }
    }
}

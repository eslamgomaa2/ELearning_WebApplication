using E_Learning.Core.Entities.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PaymentMethodConfiguration
    : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Type)
               .HasMaxLength(20)
               .IsRequired();
        builder.Property(pm => pm.StripePaymentMethodId)
       .HasMaxLength(100);

        builder.Property(pm => pm.CardLastFour)
               .HasMaxLength(4);

        builder.Property(pm => pm.CardHolderName)
               .HasMaxLength(100);

        builder.Property(pm => pm.PayPalEmail)
               .HasMaxLength(256);

        builder.Property(pm => pm.IsDefault)
               .HasDefaultValue(false);

        builder.Property(pm => pm.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(pm => pm.User)
               .WithMany()
               .HasForeignKey(pm => pm.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
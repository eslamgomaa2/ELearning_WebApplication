using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Billing
{
    public class PaymentMethod : BaseEntity
    {
        // ─── FK → AppUser ────────────────────────
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        // ─── Type ────────────────────────────────
        // CreditCard,PayPal
        public string Type { get; set; } = string.Empty;
        public string? StripePaymentMethodId { get; set; }
        // ─── Credit Card Info ────────────────────
        public string? CardLastFour { get; set; }
        public string? CardHolderName { get; set; }
        public byte? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }

        // ─── PayPal ──────────────────────────────
        public string? PayPalEmail { get; set; }

        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
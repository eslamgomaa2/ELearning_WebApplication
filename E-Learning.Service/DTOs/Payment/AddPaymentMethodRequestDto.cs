using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{

    public class AddPaymentMethodRequestDto
    {
        public string Type { get; set; } = string.Empty;       // CreditCard | PayPal
        public string? StripePaymentMethodId { get; set; }     // Stripe.js
        public string? PayPalEmail { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}

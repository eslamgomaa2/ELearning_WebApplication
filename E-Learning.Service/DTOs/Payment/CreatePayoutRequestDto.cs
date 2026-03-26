using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{
    public class CreatePayoutRequestDto
    {
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;   // BankTransfer | PayPal | Stripe
        public string AccountDetails { get; set; } = string.Empty;
    }
}

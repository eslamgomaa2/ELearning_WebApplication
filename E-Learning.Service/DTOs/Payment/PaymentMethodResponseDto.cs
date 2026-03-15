using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{
    public class PaymentMethodResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? CardLastFour { get; set; }
        public string? CardHolderName { get; set; }
        public byte? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public string? PayPalEmail { get; set; }
        public bool IsDefault { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{
    public class ConfirmPaymentRequestDto
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int? PaymentMethodId { get; set; }
        public bool SaveCard { get; set; } = false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{
    public class CreatePaymentIntentRequestDto
    {
        public int CourseId { get; set; }
        public int? PaymentMethodId { get; set; } // Saved card (optional)
    }
}

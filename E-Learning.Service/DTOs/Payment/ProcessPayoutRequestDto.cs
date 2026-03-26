using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{


    public class ProcessPayoutRequestDto
    {
        public int PayoutRequestId { get; set; }
        public string Decision { get; set; } = string.Empty;  // Approved | Rejected
        public string? Notes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{
    public class InstructorEarningsResponseDto
    {
        public decimal TotalEarnings { get; set; }
        public decimal PendingPayout { get; set; }
        public decimal AvailableForPayout { get; set; }
        public List<EarningItemDto> Earnings { get; set; } = new();
    }
}

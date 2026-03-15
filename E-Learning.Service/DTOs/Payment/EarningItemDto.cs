using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Payment
{
    public class EarningItemDto
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal GrossAmount { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal NetAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? AvailableAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

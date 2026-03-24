using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Schedule
{
    public class CalendarDayDto
    {
        public DateOnly Date { get; set; }
        public List<CalendarEventDto> Events { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.Service.DTOs.Schedule;

namespace E_Learning.Service.Contract
{
    public interface IScheduleService
    {
        Task<List<CalendarDayDto>> GetCalendarAsync(Guid studentId, int month, int year);
        Task<List<ScheduleEventDto>> GetUpcomingEventsAsync(Guid studentId, ScheduleQueryDto query);
        Task<DeadlineSummaryDto> GetDeadlineSummaryAsync(Guid studentId);
        Task<ScheduleEventDto> GetEventDetailsAsync(Guid studentId, string type, int id);
    }
}
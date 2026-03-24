using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Schedule
{
    public class DeadlineSummaryDto
    {
        public int UpcomingExams { get; set; }
        public int UpcomingAssignments { get; set; }
        public int UpcomingQuizzes { get; set; }
        public int UpcomingLiveSessions { get; set; }
        public int TotalUpcoming { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
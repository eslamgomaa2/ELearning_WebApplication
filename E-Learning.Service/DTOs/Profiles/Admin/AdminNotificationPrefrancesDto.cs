using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class AdminNotificationPrefrancesDto
    {
        public bool EmailNotifications { get; set; }  
        public bool QuizSubmissions { get; set; }   
        public bool StudentEnrollments { get; set; }   
        public bool SystemUpdates { get; set; }   
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public  class StudentNotificationSettingDto
    {
        public bool CourseAnnouncement { get; set; } = true;
        public bool AssignmentReminder { get; set; } = true;
        public bool ExamNotification { get; set; } = true;
        public bool PlatformUpdates { get; set; } = true;
        public bool EmailNotification { get; set; } = true;
        public bool In_AppNotification { get; set; } = true;

    }
}

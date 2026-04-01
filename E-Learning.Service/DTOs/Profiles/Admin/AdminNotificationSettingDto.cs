using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class AdminNotificationSettingDto
    {

        public bool CourseEnrollmentConfirmation { get; set; }
        public bool GradePublished { get; set; }
        public bool AssignmentReminders { get; set; }
        public bool WeeklyActivityDigest { get; set; }
        public bool ExamReminders { get; set; }
        public bool EmergencyAlerts { get; set; }
        public bool EmailNotification { get; set; }

        // In-Web Notifications section
        public bool InAppNotification { get; set; }

        // Automated Triggers section
        public bool LowAttendanceAlert { get; set; }
        public bool FailingGradeWarning { get; set; }
        public bool EnrollmentCapacityAlert { get; set; }

    }
}

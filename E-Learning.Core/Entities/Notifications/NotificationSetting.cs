using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Notifications
{
    public class NotificationSetting : BaseEntity
    {
        // ─── FK - One-to-One ──────────────────────
        public Guid UserId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public ApplicationUser User { get; set; } = null!;

        // ─── Types On/Off ─────────────────────────
        public bool CourseAnnouncement { get; set; } = true;
        public bool AssignmentReminder { get; set; } = true;
        public bool PlatformUpdates { get; set; } = true;
        public bool CourseEnrollmentConfirmation { get; set; } = true;
        public bool GradePublished { get; set; } = true;
        public bool WeeklyActivityDigest { get; set; } = false;
        public bool ExamNotification { get; set; } = true;
        public bool EmergencyAlerts { get; set; } = false;
        // ─── Automated Triggers ───────────────────────────────────────────────
        public bool LowAttendanceAlert { get; set; } = false;
        public bool FailingGradeWarning { get; set; } = false;
        public bool EnrollmentCapacityAlert { get; set; } = true;  // "Active" badge



        // ─── Channels On/Off ─────────────────────
        public bool InAppNotification { get; set; } = true;
        public bool EmailNotification { get; set; } = true;
        //  NewStudentEnrollmentEmail Preferences
        public bool NewStudentEnrollmentEmail { get; set; } = true;
        public bool NewStudentEnrollmentInApp { get; set; } = true;

        // Exam Submission Preferences
        public bool ExamSubmissionEmail { get; set; } = true;
        public bool QuizSubmission { get; set; } = true;
        public bool ExamSubmissionInApp { get; set; } = true;
    }
}

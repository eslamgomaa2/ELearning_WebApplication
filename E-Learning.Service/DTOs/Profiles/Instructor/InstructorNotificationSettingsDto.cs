using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Instructor
{
    public class InstructorNotificationSettingsDto
    {
        public bool NewStudentEnrollmentEmail { get; set; } = true;
        public bool NewStudentEnrollmentInApp { get; set; } = true;
        public bool ExamSubmissionEmail { get; set; } = true;
        public bool ExamSubmissionInApp { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class EnrollmentExportDto
    {
        public string CourseTitle { get; init; } = string.Empty;
        public DateTime EnrolledAt { get; init; }
        

    }
}

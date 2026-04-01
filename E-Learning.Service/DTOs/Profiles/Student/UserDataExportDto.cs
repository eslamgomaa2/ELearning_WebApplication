using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class UserDataExportDto
    {
        
            public DateTime ExportedAt { get; init; }
            public ProfileExportDto Profile { get; init; } = new();
            public List<EnrollmentExportDto> Courses { get; init; } = new();
            public List<ActivityExportDto> ActivityHistory { get; init; } = new();
        
    }
}

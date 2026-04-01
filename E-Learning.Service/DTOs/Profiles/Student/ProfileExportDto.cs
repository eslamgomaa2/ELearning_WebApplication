using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class ProfileExportDto
    {
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string? Location { get; init; }
        public DateOnly? DateOfBirth { get; init; }
        public DateTime MemberSince { get; init; }
        public string Language { get; init; } = string.Empty;
        public string TimeZone { get; init; } = string.Empty;

    }
}

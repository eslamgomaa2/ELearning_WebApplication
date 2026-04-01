using E_Learning.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class ActivityExportDto
    {
        public string Title { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
        public NotificationType Type { get; init; } 
        public bool IsRead { get; init; }
        public DateTime CreatedAt { get; init; }

    }
}

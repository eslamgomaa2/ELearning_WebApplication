using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Profiles
{
    public class StudentProfile : BaseEntity
    {
        public Guid AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; } = null!;

        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Location { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? LevelId { get; set; }
        public Level? Level { get; set; }
        
        public decimal EngagementRate { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

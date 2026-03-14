using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Profiles
{
    public class AdminProfile : BaseEntity
    {
        public Guid AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; } = null!;
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Location { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool IsSuperAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Profiles
{
    public class InstructorProfile : BaseEntity
    {
        public Guid AppUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser AppUser { get; set; } = null!;

        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Location { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Headline { get; set; }
        public string? phoneNumber { get; set; }
        public string? About { get; set; }
        public string? Gender { get; set; }
        public decimal TotalEarnings { get; set; } = 0;
        public decimal PendingPayout { get; set; } = 0;

        public bool IsPublic { get; set; } = true;
        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

 
    }
}

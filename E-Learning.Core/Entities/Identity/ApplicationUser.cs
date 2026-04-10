using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace E_learning.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // ─── Basic Info ──────────────────────────
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime MemberSince { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //// ─── Auth Provider ───────────────────────
        //public string AuthProvider { get; set; } = "Local";
        //public string? GoogleId { get; set; }

        // ─── Settings  ─────────
        public string Language { get; set; } = "en";
        public string TimeZone { get; set; } = "UTC";

        // ─── Privacy ─────────────────────────────
        public bool ProfileVisibility { get; set; } = true;
        public bool ShowProgressToOthers { get; set; } = true;

        
        public bool IsApproved { get; set; } = true;  // false for instructors until admin approves
        public InstructorStatus? InstructorStatus { get; set; }  

        // ─── NavProp ─────────────────────────────
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public NotificationSetting? NotificationSetting { get; set; }


    }
}
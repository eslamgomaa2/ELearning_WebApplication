using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace E_Learning.Core.Entities.Courses
{
    public class Course : AuditableEntity, ISoftDelete
    {
        // FK → AppUser (Instructor)
        public Guid InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; } = null!;

        public int? LevelId { get; set; }
        public Level? Level { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? WhatYouWillLearn { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Language { get; set; } = "en";
        public decimal Price { get; set; } = 0;
        public int? DurationInMinutes { get; set; }
        public string Status { get; set; } = "Draft";
        public bool IsActive { get; set; } = true;

        // Course Approval
        public Guid? ApprovedBy { get; set; }
        public ApplicationUser? ApprovedByUser { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? RejectionNote { get; set; }

        // ISoftDelete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.ExamsDtos
{
    public class ExamDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public string? Rules { get; set; }
        public string? TechnicalRequirements { get; set; }
        public string? EducationLevel { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int DurationSeconds { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingScore { get; set; }
        public int MaxAttempts { get; set; }
        public bool AIShuffleEnabled { get; set; }
        public string? SourceFileUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

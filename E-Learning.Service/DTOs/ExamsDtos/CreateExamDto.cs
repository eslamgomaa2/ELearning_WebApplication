using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Service.DTOs.ExamsDtos
{
    public class CreateExamDto
    {
        [Required(ErrorMessage = "CourseId is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Instructions cannot exceed 1000 characters.")]
        public string? Instructions { get; set; }

        [MaxLength(1000, ErrorMessage = "Rules cannot exceed 1000 characters.")]
        public string? Rules { get; set; }

        [MaxLength(1000, ErrorMessage = "Technical requirements cannot exceed 1000 characters.")]
        public string? TechnicalRequirements { get; set; }

        [MaxLength(100, ErrorMessage = "Education level cannot exceed 100 characters.")]
        public string? EducationLevel { get; set; }

        [Required(ErrorMessage = "ScheduledAt is required.")]
        public DateTime ScheduledAt { get; set; }

        public DateTime? EndDateTime { get; set; }

        [Required(ErrorMessage = "DurationSeconds is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 second.")]
        public int DurationSeconds { get; set; }

        [Required(ErrorMessage = "TotalMarks is required.")]
        [Range(0, 1000, ErrorMessage = "Total marks must be between 0 and 1000.")]
        public decimal TotalMarks { get; set; }

        [Range(0, 100, ErrorMessage = "Passing score must be between 0 and 100.")]
        public decimal PassingScore { get; set; } = 60;

        [Range(1, 10, ErrorMessage = "Max attempts must be between 1 and 10.")]
        public int MaxAttempts { get; set; } = 1;

        public bool AIShuffleEnabled { get; set; } = true;

        //[Url(ErrorMessage = "SourceFileUrl must be a valid URL.")]
        public IFormFile? File { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

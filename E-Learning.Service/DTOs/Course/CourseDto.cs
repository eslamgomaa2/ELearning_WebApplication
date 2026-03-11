using E_Learning.Service.DTOs.Section;

namespace E_Learning.Service.DTOs.Course
{
    public class CourseDto
    {
        public int Id { get; set; }

        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;

        public int? LevelId { get; set; }
        public string? LevelName { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? WhatYouWillLearn { get; set; }
        public string? ThumbnailUrl { get; set; }

        public string Language { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? DurationInMinutes { get; set; }

        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<SectionDto> Sections { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.CourseDto
{
    public class CreateCourseDto
    {
        public Guid InstructorId { get; set; }
        public int? LevelId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? WhatYouWillLearn { get; set; }
        public string? ThumbnailUrl { get; set; }

        public string Language { get; set; } = "en";
        public decimal Price { get; set; }
        public int? DurationInMinutes { get; set; }
    }
}

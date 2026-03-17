using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs
{
    public class QuizResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Topic { get; set; }
        public string Type { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int? LessonId { get; set; }
        public int? TimeLimitSeconds { get; set; }
        public int TimePerQuestionSeconds { get; set; }
        public decimal PassingScore { get; set; }
        public int MaxAttempts { get; set; }
        public bool ShuffleQuestions { get; set; }
        public bool ShowResultsImmediately { get; set; }
        public bool IsActive { get; set; }
        public int QuestionsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

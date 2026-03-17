using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Assessments.Quiz
{
    public class Quiz : AuditableEntity
    {
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Topic { get; set; }
        public string Type { get; set; } = "Regular";
        public int? TimeLimitSeconds { get; set; }
        public int TimePerQuestionSeconds { get; set; } = 30;
        public decimal PassingScore { get; set; } = 60;
        public int MaxAttempts { get; set; } = 3;
        public bool ShuffleQuestions { get; set; } = true;
        public bool ShowResultsImmediately { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public DateTime? ScheduledAt { get; set; }
        public ICollection<QuizQuestion> Questions { get; set; }
            = new List<QuizQuestion>();
        public ICollection<QuizAttempt> Attempts { get; set; }
            = new List<QuizAttempt>();
    }
}

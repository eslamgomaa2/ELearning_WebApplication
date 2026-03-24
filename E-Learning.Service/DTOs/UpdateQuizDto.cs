using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs
{
    public class UpdateQuizDto
    {
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
    }
}

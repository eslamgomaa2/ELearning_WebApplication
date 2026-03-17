using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs
{
    public class QuizQuestionResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public int OrderIndex { get; set; }
        public List<QuizOptionResponseDto> Options { get; set; } = new();
    }
}

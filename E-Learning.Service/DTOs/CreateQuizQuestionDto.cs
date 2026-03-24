using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs
{
    public class CreateQuizQuestionDto
    {
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = "MCQ";
        public decimal Points { get; set; } = 1;
        public int OrderIndex { get; set; }
        public List<CreateQuizOptionDto> Options { get; set; } = new();
    }
}

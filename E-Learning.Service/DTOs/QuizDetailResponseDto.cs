using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs
{
    public class QuizDetailResponseDto : QuizResponseDto
    {
        public List<QuizQuestionResponseDto> Questions { get; set; } = new();
    }
}

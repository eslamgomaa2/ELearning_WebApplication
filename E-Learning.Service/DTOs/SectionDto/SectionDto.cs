using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.SectionDto
{
    public class SectionDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        public List<LessonDto> Lessons { get; set; } = new();
    }
}

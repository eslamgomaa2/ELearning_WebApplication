using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.AssignmentsDto
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }
        public decimal TotalMarks { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

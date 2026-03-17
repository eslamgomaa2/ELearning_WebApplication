using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Features.Courses.Queries
{
    public class CourseQuery
    {
        public Guid? InstructorId { get; set; }
        public Guid? StudentId { get; set; }
        public int? LevelId { get; set; }
        public string? Subject { get; set; }
        public string? StageType { get; set; }
        public string? Status { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

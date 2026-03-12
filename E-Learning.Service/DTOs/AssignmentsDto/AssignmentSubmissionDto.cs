using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.Core.Entities.Assessments.Assignments;

namespace E_Learning.Service.DTOs.AssignmentsDto
{
    public class AssignmentSubmissionDto
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }

        //public Assignment Assignment { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
       
        public DateTime? SubmittedAt { get; set; }

        public string? FileUrl { get; set; }

        public string? Notes { get; set; }

        public decimal? Score { get; set; }

        public string? TeacherComment { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

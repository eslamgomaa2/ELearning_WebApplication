using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.AssignmentsDto
{
    public class GradeSubmissionDto
    {
        [Required(ErrorMessage = "Score is required")]
        [Range(0, 1000, ErrorMessage = "Score must be between 0 and 1000")]
        public decimal Score { get; set; }

        [MaxLength(500, ErrorMessage = "Teacher comment cannot exceed 500 characters")]
        public string? TeacherComment { get; set; }
    }
}

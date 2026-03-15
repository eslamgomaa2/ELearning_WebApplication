using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.ExamsDtos
{
    public class UpdateExamDto : CreateExamDto
    {
        [Required(ErrorMessage = "Exam Id is required.")]
        public int Id { get; set; }
    }
}

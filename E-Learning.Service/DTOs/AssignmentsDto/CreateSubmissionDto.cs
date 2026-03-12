using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Service.DTOs.AssignmentsDto
{
    public class CreateSubmissionDto
    {

        [Required(ErrorMessage = "AssignmentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "AssignmentId must be greater than 0.")]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "StudentId is required.")]
        public Guid StudentId { get; set; }


        public IFormFile? File { get; set; }
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }
    }
}

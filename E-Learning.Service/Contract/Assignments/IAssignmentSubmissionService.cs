using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Service.DTOs.AssignmentsDto;

namespace E_Learning.Service.Contract.Assignments
{
    public interface IAssignmentSubmissionService
    {
        Task<Response<AssignmentSubmissionDto>> CreateSubmitAsync(CreateSubmissionDto dto);

        Task<Response<AssignmentSubmissionDto>> CreateGradeAsync(int submissionId, GradeSubmissionDto dto);

        Task<Response<IReadOnlyList<AssignmentSubmissionDto>>> GetAllByAssignmentAsync(int assignmentId);

        Task<Response<IReadOnlyList<AssignmentSubmissionDto>>>   GetByStudentAsync(Guid studentId);
    }
}

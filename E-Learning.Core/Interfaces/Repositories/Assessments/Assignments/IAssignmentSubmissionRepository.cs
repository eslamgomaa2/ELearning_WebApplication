using E_Learning.Core.Entities.Assessments.Assignments;

namespace E_Learning.Core.Interfaces.Repositories.Assessments.Assignments
{
    public interface IAssignmentSubmissionRepository   : IGenericRepository<AssignmentSubmission, int>
    {
      Task <AssignmentSubmission?> GetAssignmentSubmissionByIdWithAssimentData(int id);
        Task<IReadOnlyList<AssignmentSubmission>> GetByStudentIdAsync(Guid studentId);
        Task<IReadOnlyList<AssignmentSubmission>> GetByAssignmentIdAsync(int assignmentId);
    }
}
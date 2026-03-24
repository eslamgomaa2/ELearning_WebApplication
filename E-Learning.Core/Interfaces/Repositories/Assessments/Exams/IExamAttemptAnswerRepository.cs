using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.Core.Repository
{
    public interface IExamAttemptAnswerRepository : IGenericRepository<ExamAttemptAnswer,int>
    {
        // Get all answers for a specific attempt with full details
        Task<IReadOnlyList<ExamAttemptAnswer>> GetByAttemptIdAsync( int attemptId, CancellationToken ct = default);

        // Get single answer with question + option details
        Task<ExamAttemptAnswer?> GetByIdWithDetailsAsync( int answerId, CancellationToken ct = default);

        // Get a specific answer by attemptId + questionId
        Task<ExamAttemptAnswer?> GetByAttemptAndQuestionAsync(int attemptId, int questionId,CancellationToken ct = default);

        // Get all text-type answers for an attempt (need manual grading)
        Task<IReadOnlyList<ExamAttemptAnswer>> GetTextAnswersByAttemptAsync( int attemptId,  CancellationToken ct = default);

        // Check answer belongs to attempt
        Task<bool> ExistsAsync(
            int attemptId,
            int answerId,
            CancellationToken ct = default);
    }
}
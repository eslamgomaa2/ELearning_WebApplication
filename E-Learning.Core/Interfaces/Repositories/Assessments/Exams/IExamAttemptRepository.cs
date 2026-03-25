using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.Core.Repository
{
    public interface IExamAttemptRepository : IGenericRepository<ExamAttempt,int>
    {
        // Get attempt with answers + questions + options
        Task<ExamAttempt?> GetByIdWithDetailsAsync(int attemptId, CancellationToken ct = default);

        // Get all attempts for an exam (instructor view)
        Task<IReadOnlyList<ExamAttempt>> GetByExamIdAsync(int examId,PaginationParams paginationParams, CancellationToken ct = default);

        // Get all attempts for a student on a specific exam
        Task<IReadOnlyList<ExamAttempt>> GetByStudentAndExamAsync(Guid studentId, int examId, CancellationToken ct = default);

        // Get active in-progress attempt for a student on an exam
        Task<ExamAttempt?> GetActiveAttemptAsync(Guid studentId, int examId, CancellationToken ct = default);

        // Count how many attempts a student made on an exam
        Task<int> CountAttemptsAsync(Guid studentId, int examId, CancellationToken ct = default);

        // Get attempt with exam info (for submit — needs DurationSeconds)
        Task<ExamAttempt?> GetByIdWithExamAsync(int attemptId, CancellationToken ct = default);
    }
}
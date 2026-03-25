using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;

namespace E_Learning.Service.Services.ExamServices.Attempts
{
    public interface IExamAttemptServices
    {
        // Student starts a new attempt
        Task<Response<ExamAttempt>> StartAsync(int examId, Guid studentId, CancellationToken ct);

        // Student submits answers
        Task<Response<ExamAttempt>> SubmitAsync(int examId, int attemptId, Guid studentId, SubmitAttemptDto dto, CancellationToken ct);

        // Get single attempt detail
        Task<Response<ExamAttempt>> GetByIdAsync(int examId, int attemptId, CancellationToken ct);

        // Student gets their own attempts on an exam
        Task<Response<IReadOnlyList<ExamAttempt>>> GetMyAttemptsAsync(int examId, Guid studentId, CancellationToken ct);

        // Instructor gets all attempts for an exam
        Task<Response<IReadOnlyList<ExamAttempt>>> GetAllByExamAsync(int examId, PaginationParams paginationParams, CancellationToken ct);

        // Instructor reviews an attempt
        Task<Response<ExamAttempt >> ReviewAsync(int examId, int attemptId, Guid reviewerId, ReviewAttemptDto dto, CancellationToken ct);

        // Instructor publishes result to student
        Task<Response<ExamAttempt>> PublishAsync(int examId, int attemptId, CancellationToken ct);
    }
}
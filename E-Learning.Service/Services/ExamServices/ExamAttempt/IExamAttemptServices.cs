using E_Learning.Core.Base;

namespace E_Learning.Service.Services.ExamServices.Attempts
{
    public interface IExamAttemptServices
    {
        // Student starts a new attempt
        Task<Response<StartAttemptResponseDto>> StartAsync(int examId, Guid studentId, CancellationToken ct);

        // Student submits answers
        Task<Response<AttemptResponseDto>> SubmitAsync(int examId, int attemptId, Guid studentId, SubmitAttemptDto dto, CancellationToken ct);

        // Get single attempt detail
        Task<Response<AttemptResponseDto>> GetByIdAsync(int examId, int attemptId, CancellationToken ct);

        // Student gets their own attempts on an exam
        Task<Response<IReadOnlyList<AttemptResponseDto>>> GetMyAttemptsAsync(int examId, Guid studentId, CancellationToken ct);

        // Instructor gets all attempts for an exam
        Task<Response<IReadOnlyList<AttemptResponseDto>>> GetAllByExamAsync(int examId, CancellationToken ct);

        // Instructor reviews an attempt
        Task<Response<AttemptResponseDto>> ReviewAsync(int examId, int attemptId, Guid reviewerId, ReviewAttemptDto dto, CancellationToken ct);

        // Instructor publishes result to student
        Task<Response<string>> PublishAsync(int examId, int attemptId, CancellationToken ct);
    }
}
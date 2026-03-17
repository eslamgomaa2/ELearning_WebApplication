using E_Learning.Core.Base;

namespace E_Learning.Service.Services.ExamServices.Answers
{
    public interface IExamAttemptAnswerServices
    {
        // Get all answers for an attempt
        Task<Response<IReadOnlyList<AttemptAnswerResponseDto>>> GetByAttemptAsync(
            int examId,
            int attemptId,
            CancellationToken ct);

        // Get single answer detail
        Task<Response<AttemptAnswerResponseDto>> GetByIdAsync(
            int examId,
            int attemptId,
            int answerId,
            CancellationToken ct);

        // Get only text answers needing manual grading
        Task<Response<IReadOnlyList<AttemptAnswerResponseDto>>> GetTextAnswersAsync(
            int examId,
            int attemptId,
            CancellationToken ct);

        // Instructor grades a single text answer
        Task<Response<AttemptAnswerResponseDto>> GradeAnswerAsync(
            int examId,
            int attemptId,
            int questionId,
            UpdateAnswerScoreDto dto,
            CancellationToken ct);

        // Instructor grades all text answers at once
        Task<Response<string>> BulkGradeAsync(
            int examId,
            int attemptId,
            BulkUpdateAnswerScoreDto dto,
            CancellationToken ct);
    }
}
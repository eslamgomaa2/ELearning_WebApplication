using E_Learning.Core.Base;

namespace E_Learning.Service.Services.ExamServices.Questions
{
    public interface IExamQuestionServices
    {
        Task<Response<AddQuestionsResponseDto>> AddManuallyAsync(int examId, AddQuestionsDto dto, CancellationToken ct);
        Task<Response<IReadOnlyList<QuestionResponseDto>>> GetQuestionsByExamIdAsync(int examId, CancellationToken ct);
        Task<Response<QuestionResponseDto>> UpdateAsync(int examId, int questionId, UpdateQuestionDto dto, CancellationToken ct);
        Task<Response<string>> DeleteAsync(int examId, int questionId, CancellationToken ct);
        Task<Response<string>> ReorderAsync(int examId, ReorderQuestionsDto dto, CancellationToken ct);
    }
}
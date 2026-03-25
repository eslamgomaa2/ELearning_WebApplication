using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;

namespace E_Learning.Service.Services.ExamServices.Questions
{
    public interface IExamQuestionServices
    {
        Task<Response<IReadOnlyList<ExamQuestion>>> AddManuallyAsync(int examId, AddQuestionsDto dto, CancellationToken ct);
        Task<Response<IReadOnlyList<ExamQuestion>>> GetQuestionsByExamIdAsync(int examId,PaginationParams paginationParams, CancellationToken ct);
        Task<Response<ExamQuestion>> UpdateAsync(int examId, int questionId, UpdateQuestionDto dto, CancellationToken ct);
        Task<Response<ExamQuestion>> DeleteAsync(int examId, int questionId, CancellationToken ct);
        Task<Response<string>> ReorderAsync(int examId, ReorderQuestionsDto dto, CancellationToken ct);
    }
}
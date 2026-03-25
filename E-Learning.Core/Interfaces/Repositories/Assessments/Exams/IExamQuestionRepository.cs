using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.Core.Repository
{
    public interface IExamQuestionRepository : IGenericRepository<ExamQuestion,int>
    {
        
        Task<IReadOnlyList<ExamQuestion>> GetByExamIdAsync(int examId,PaginationParams paginationParams, CancellationToken ct = default);

        
        Task<ExamQuestion?> GetByIdWithOptionsAsync(int questionId, CancellationToken ct = default);

        
        Task<bool> ExistsAsync(int examId, int questionId, CancellationToken ct = default);

        
        Task<int> GetMaxOrderIndexAsync(int examId, CancellationToken ct = default);

       
        Task AddRangeAsync(IEnumerable<ExamQuestion> questions, CancellationToken ct = default);

        
        Task<IReadOnlyList<ExamQuestion>> GetByIdsAsync(int examId, IEnumerable<int> ids, CancellationToken ct = default);
    }
}
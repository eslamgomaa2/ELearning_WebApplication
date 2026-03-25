using E_Learning.core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Repository;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Exams
{
    public class ExamQuestionRepository :GenericRepository<ExamQuestion,int>, IExamQuestionRepository
    {
        public ELearningDbContext _context { get; }
        public ExamQuestionRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ExamQuestion>> GetByExamIdAsync(int examId,PaginationParams paginationParams, CancellationToken ct = default)
        {
            return await _context.ExamQuestions
                .Where(q => q.ExamId == examId)
                .OrderBy(q => q.OrderIndex)
                .Include(q => q.Options.OrderBy(o => o.OrderIndex))
                .AsNoTracking()
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync(ct);
        }

        public async Task<ExamQuestion?> GetByIdWithOptionsAsync(int questionId, CancellationToken ct = default)
        {
            return await _context.ExamQuestions
                .Include(q => q.Options.OrderBy(o => o.OrderIndex))
                .FirstOrDefaultAsync(q => q.Id == questionId, ct);
        }

        public async Task<bool> ExistsAsync(int examId, int questionId, CancellationToken ct = default)
        {
            return await _context.ExamQuestions
                .AnyAsync(q => q.Id == questionId && q.ExamId == examId, ct);
        }

        public async Task<int> GetMaxOrderIndexAsync(int examId, CancellationToken ct = default)
        {
            var max = await _context.ExamQuestions
                .Where(q => q.ExamId == examId)
                .MaxAsync(q => (int?)q.OrderIndex, ct);

            return max ?? 0;
        }

        

        public async Task<IReadOnlyList<ExamQuestion>> GetByIdsAsync(int examId, IEnumerable<int> ids, CancellationToken ct = default)
        {
            return await _context.ExamQuestions
                .Where(q => q.ExamId == examId && ids.Contains(q.Id))
                .ToListAsync(ct);
        }
    }
}

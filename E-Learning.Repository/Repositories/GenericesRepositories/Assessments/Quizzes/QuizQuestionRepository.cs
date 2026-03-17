using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Interfaces.Repositories.Assessments.Quizzes;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Quizzes
{
    public class QuizQuestionRepository : GenericRepository<QuizQuestion, int>, IQuizQuestionRepository
    {
        private readonly ELearningDbContext _context;

        public QuizQuestionRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<QuizQuestion>> GetByQuizIdAsync(int quizId, CancellationToken ct = default)
        {
            return await _context.QuizQuestions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Options.OrderBy(o => o.OrderIndex))
                .OrderBy(q => q.OrderIndex)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<QuizQuestion?> GetWithOptionsAsync(int questionId, CancellationToken ct = default)
        {
            return await _context.QuizQuestions
                .Include(q => q.Options.OrderBy(o => o.OrderIndex))
                .FirstOrDefaultAsync(q => q.Id == questionId, ct);
        }
    }
}
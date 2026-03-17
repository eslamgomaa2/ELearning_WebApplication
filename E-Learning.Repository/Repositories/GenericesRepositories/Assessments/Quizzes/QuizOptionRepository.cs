using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Interfaces.Repositories.Assessments.Quizzes;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Quizzes
{
    public class QuizOptionRepository : GenericRepository<QuizOption, int>, IQuizOptionRepository
    {
        private readonly ELearningDbContext _context;

        public QuizOptionRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<QuizOption>> GetByQuestionIdAsync(int questionId, CancellationToken ct = default)
        {
            return await _context.QuizOptions
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.OrderIndex)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
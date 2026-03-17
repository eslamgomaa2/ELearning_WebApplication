using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Interfaces.Repositories.Assessments.Quizzes;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;
using Microsoft.EntityFrameworkCore;
using E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Quizzes;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Quizzes
{
    public class QuizRepository : GenericRepository<Quiz, int>, IQuizRepository
    {
        private readonly ELearningDbContext _context;

        public QuizRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Quiz?> GetWithQuestionsAndOptionsAsync(int quizId, CancellationToken ct = default)
        {
            return await _context.Quizzes
                .Include(q => q.Questions.OrderBy(qq => qq.OrderIndex))
                    .ThenInclude(qq => qq.Options.OrderBy(o => o.OrderIndex))
                .FirstOrDefaultAsync(q => q.Id == quizId, ct);
        }

        public async Task<IReadOnlyList<Quiz>> GetByCourseIdAsync(int courseId, CancellationToken ct = default)
        {
            return await _context.Quizzes
                .Where(q => q.CourseId == courseId)
                .Include(q => q.Questions)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Quizzes.AnyAsync(q => q.Id == id, ct);
        }

        public async Task<bool> ExistsByTitleAsync(string title, int courseId, CancellationToken ct = default)
        {
            return await _context.Quizzes
                .AnyAsync(q => q.Title.ToLower() == title.ToLower() && q.CourseId == courseId, ct);
        }
    }
}
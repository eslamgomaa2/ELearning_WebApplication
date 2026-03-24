using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.Core.Interfaces.Repositories.Assessments.Quizzes
{
    public interface IQuizRepository : IGenericRepository<Quiz, int>
    {
        Task<Quiz?> GetWithQuestionsAndOptionsAsync(int quizId, CancellationToken ct = default);
        Task<IReadOnlyList<Quiz>> GetByCourseIdAsync(int courseId, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByTitleAsync(string title, int courseId, CancellationToken ct = default);
    }
}
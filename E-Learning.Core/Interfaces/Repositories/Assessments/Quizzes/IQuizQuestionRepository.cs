using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.Core.Interfaces.Repositories.Assessments.Quizzes
{
    public interface IQuizQuestionRepository : IGenericRepository<QuizQuestion, int>
    {
        Task<IReadOnlyList<QuizQuestion>> GetByQuizIdAsync(int quizId, CancellationToken ct = default);
        Task<QuizQuestion?> GetWithOptionsAsync(int questionId, CancellationToken ct = default);
    }
}
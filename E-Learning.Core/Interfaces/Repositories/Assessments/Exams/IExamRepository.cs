using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.core.Interfaces.Repositories.Assessments.Exams
{
    public interface IExamRepository:IGenericRepository<Exam,int>
    {
        Task<Exam?> GetByIdWithQuestionsAsync(int examId, CancellationToken ct = default);

    }
}
using E_Learning.core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Exams
{
    public class ExamRepository :GenericRepository<Exam,int>,IExamRepository
    {
        public ELearningDbContext _context { get; }
        public ExamRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Exam?> GetByIdWithQuestionsAsync(int examId, CancellationToken ct = default)
        {
            return await _context.Exams
        .Include(e => e.Questions.OrderBy(q => q.OrderIndex))
            .ThenInclude(q => q.Options.OrderBy(o => o.OrderIndex))
        .FirstOrDefaultAsync(e => e.Id == examId, ct);
        }
    }
}

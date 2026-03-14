using E_Learning.core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Exams
{
    public class ExamRepository :GenericRepository<Exam,int>, IExamRepository
    {
        public ExamRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }

        public async Task<IEnumerable<Exam>> GetExamsByCourseIdAsync(int courseId)
        {
            return await _context.Exams.Where(e => e.CourseId == courseId)
                .Include(e => e.Course).ToListAsync();
        }
    }
}

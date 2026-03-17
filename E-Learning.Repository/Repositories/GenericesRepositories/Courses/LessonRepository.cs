using E_Learning.core.Interfaces.Repositories.Courses;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Courses
{
    public class LessonRepository: GenericRepository<Lesson,int>, ILessonRepository
    {
        public LessonRepository(ELearningDbContext context) : base(context) 
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }

        private IQueryable<Lesson> WithFullIncludes()
        {
            return _context.Lessons
                .Include(e => e.Section)
                .ThenInclude(e => e.Course);
        }

        public async Task<IReadOnlyList<Lesson>> GetLessonsBySectionIdAsync(int sectionId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(l => l.SectionId == sectionId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Lesson>> GetLessonsByCourseIdAsync(int courseId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(l => l.Section.CourseId == courseId)
                .ToListAsync();
        }

        
    }
}

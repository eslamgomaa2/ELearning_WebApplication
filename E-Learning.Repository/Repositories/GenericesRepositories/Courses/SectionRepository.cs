using E_Learning.core.Interfaces.Repositories.Courses;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Courses
{
    public class SectionRepository: GenericRepository<Section,int> ,ISectionRepository
    {
        public SectionRepository(ELearningDbContext context):base(context) 
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }

        private IQueryable<Section> WithFullIncludes()
        {
            return _context.Sections
                .Include(e => e.Course)
                .Include(e => e.Lessons);
        }

        public async Task<IReadOnlyList<Section>> GetSectionsByCourseIdAsync(int courseId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(s => s.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}

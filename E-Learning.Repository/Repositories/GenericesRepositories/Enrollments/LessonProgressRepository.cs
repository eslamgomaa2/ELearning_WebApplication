using E_Learning.Core.Entities.Enrollment;
using E_Learning.Core.Interfaces.Repositories.Enrollments;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Enrollments
{
    public class LessonProgressRepository : ILessonProgressRepository
    {
        private readonly ELearningDbContext _context;

        public LessonProgressRepository(ELearningDbContext context)
        {
            _context = context;
        }

        private IQueryable<LessonProgress> WithFullIncludes()
        {
            return _context.LessonProgresses
                .Include(lp => lp.Lesson)
                .Include(lp => lp.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(lp => lp.Enrollment)
                    .ThenInclude(e => e.Course)
                .Include(lp => lp.Enrollment)
                    .ThenInclude(e => e.Transaction);
        }


        public async Task<LessonProgress?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(lp => lp.Id == id, ct);
        }

        public async Task<IReadOnlyList<LessonProgress>> GetByEnrollmentIdAsync(int enrollmentId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(lp => lp.EnrollmentId == enrollmentId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<LessonProgress?> GetByEnrollmentAndLessonAsync(int enrollmentId, int lessonId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(lp => lp.EnrollmentId == enrollmentId && lp.LessonId == lessonId, ct);
        }

        public async Task<bool> ExistsAsync(int enrollmentId, int lessonId, CancellationToken ct = default)
        {
            return await _context.LessonProgresses
                .AnyAsync(lp => lp.EnrollmentId == enrollmentId && lp.LessonId == lessonId, ct);
        }


        public async Task AddAsync(LessonProgress lessonProgress, CancellationToken ct = default)
        {
            await _context.LessonProgresses.AddAsync(lessonProgress, ct);
        }

        public void Update(LessonProgress lessonProgress)
        {
            _context.LessonProgresses.Update(lessonProgress);
        }

        public void Delete(LessonProgress lessonProgress)
        {
            _context.LessonProgresses.Remove(lessonProgress);
        }
    }
}
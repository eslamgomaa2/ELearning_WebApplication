using E_Learning.Core.Entities.Enrollment;
using E_Learning.Core.Interfaces.Repositories.Enrollments;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Enrollments
{
    public class EnrollmentRepository : GenericRepository<Enrollment , int> , IEnrollmentRepository
    {
        private readonly ELearningDbContext _context;

        public EnrollmentRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }

        private IQueryable<Enrollment> WithFullIncludes()
        {
            return _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Include(e => e.Transaction);
        }


        public async Task<Enrollment?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, ct);
        }

        public async Task<IReadOnlyList<Enrollment>> GetAllAsync(CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(e => !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Enrollment>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(e => e.StudentId == studentId && !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Enrollment>> GetByCourseIdAsync(int courseId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(e => e.CourseId == courseId && !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(Guid studentId, int courseId, CancellationToken ct = default)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId && !e.IsDeleted, ct);
        }


        public async Task AddAsync(Enrollment enrollment, CancellationToken ct = default)
        {
            await _context.Enrollments.AddAsync(enrollment, ct);
        }

        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }

        public void SoftDelete(Enrollment enrollment, string deletedBy)
        {
            enrollment.IsDeleted = true;
            enrollment.DeletedAt = DateTime.UtcNow;
            enrollment.DeletedBy = deletedBy;
            _context.Enrollments.Update(enrollment);
        }
    }
}
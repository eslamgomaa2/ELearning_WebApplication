using E_Learning.Core.Entities.Reviews;
using E_Learning.Core.Interfaces.Repositories.Reviews_Certificates;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Reviews_Certificates
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ELearningDbContext _context;

        public CertificateRepository(ELearningDbContext context)
        {
            _context = context;
        }


        private IQueryable<Certificate> WithFullIncludes()
        {
            return _context.Certificates
                .Include(c => c.Student)
                .Include(c => c.Course);
        }


        public async Task<Certificate?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }


        public async Task<IReadOnlyList<Certificate>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(c => c.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync(ct);
        }


        public async Task<IReadOnlyList<Certificate>> GetByCourseIdAsync(int courseId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(c => c.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync(ct);
        }


        public async Task<bool> ExistsAsync(Guid studentId, int courseId, CancellationToken ct = default)
        {
            return await _context.Certificates
                .AnyAsync(c => c.StudentId == studentId && c.CourseId == courseId, ct);
        }


        public async Task AddAsync(Certificate certificate, CancellationToken ct = default)
        {
            await _context.Certificates.AddAsync(certificate, ct);
        }


        public void Update(Certificate certificate)
        {
            _context.Certificates.Update(certificate);
        }


        public void Delete(Certificate certificate)
        {
            _context.Certificates.Remove(certificate);
        }
    }
}
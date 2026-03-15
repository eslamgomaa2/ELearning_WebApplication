using E_Learning.Core.Entities.Reviews;

namespace E_Learning.Core.Interfaces.Repositories.Reviews_Certificates
{
    public interface ICertificateRepository
    {
        Task<Certificate?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Certificate>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);
        Task<IReadOnlyList<Certificate>> GetByCourseIdAsync(int courseId, CancellationToken ct = default);
        Task AddAsync(Certificate certificate, CancellationToken ct = default);
        void Update(Certificate certificate);
        void Delete(Certificate certificate);
    }
}
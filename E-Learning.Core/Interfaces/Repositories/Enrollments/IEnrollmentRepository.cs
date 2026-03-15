using E_Learning.Core.Entities.Enrollment;

namespace E_Learning.Core.Interfaces.Repositories.Enrollments
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment, int>
    {

        Task<Enrollment?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<IReadOnlyList<Enrollment>> GetAllAsync(CancellationToken ct = default);

        Task<IReadOnlyList<Enrollment>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);

        Task<IReadOnlyList<Enrollment>> GetByCourseIdAsync(int courseId, CancellationToken ct = default);

        Task<bool> ExistsAsync(Guid studentId, int courseId, CancellationToken ct = default);

        Task AddAsync(Enrollment enrollment, CancellationToken ct = default);
        void Update(Enrollment enrollment);

        void SoftDelete(Enrollment enrollment, string deletedBy);
    }
}
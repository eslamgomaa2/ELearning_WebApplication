using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Enrollments.Enrollment;

namespace E_Learning.Core.Interfaces.Services.Enrollments
{
    public interface IEnrollmentService
    {
        Task<Response<EnrollmentResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Response<IReadOnlyList<EnrollmentResponseDto>>> GetAllAsync(CancellationToken ct = default);
        Task<Response<IReadOnlyList<EnrollmentResponseDto>>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);
        Task<Response<IReadOnlyList<EnrollmentResponseDto>>> GetByCourseIdAsync(int courseId, CancellationToken ct = default);
        Task<Response<EnrollmentResponseDto>> CreateAsync(CreateEnrollmentDto dto, CancellationToken ct = default);
        Task<Response<EnrollmentResponseDto>> UpdateAsync(int id, UpdateEnrollmentDto dto, CancellationToken ct = default);
        Task<Response<string>> SoftDeleteAsync(int id, string deletedBy, CancellationToken ct = default);
    }
}
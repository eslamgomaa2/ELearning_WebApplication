using E_Learning.Core.Base;
using E_Learning.Core.Entities.Reviews;
using E_Learning.Service.DTOs.Reviews_Certificates.Certificates;

namespace E_Learning.Core.Interfaces.Services.Reviews_Certificates
{
    public interface ICertificateServices
    {
        Task<Response<CertificateResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Response<IReadOnlyList<CertificateResponseDto>>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);
        Task<Response<IReadOnlyList<CertificateResponseDto>>> GetByCourseIdAsync(int courseId, CancellationToken ct = default);
        Task<Response<Certificate>> CreateAsync(CreateCertificateDto dto, CancellationToken ct = default);
        Task<Response<CertificateResponseDto>> UpdateAsync(int id, UpdateCertificateDto dto, CancellationToken ct = default);
        Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
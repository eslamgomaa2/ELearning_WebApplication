using E_Learning.Core.Base;
using E_Learning.Core.Entities.Reviews;
using E_Learning.Core.Interfaces.Services.Reviews_Certificates;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Reviews_Certificates.Certificates;

namespace E_Learning.Service.Services.Reviews_Certificates
{
    public class CertificateServices : ICertificateServices
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;

        public CertificateServices(IUnitOfWork uow, ResponseHandler responseHandler)
        {
            _uow = uow;
            _responseHandler = responseHandler;
        }

        public async Task<Response<CertificateResponseDto>> GetByIdAsync(
            int id, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<CertificateResponseDto>(
                    $"Certificate with ID {id} was not found.");

            return _responseHandler.Success(MapToDto(certificate));
        }

        
        public async Task<Response<IReadOnlyList<CertificateResponseDto>>> GetByStudentIdAsync(
            Guid studentId, CancellationToken ct = default)
        {
            
            var studentExists = await _uow.AppUserRepository.AnyAsync(u => u.Id == studentId, ct);
            if (!studentExists)
                return _responseHandler.NotFound<IReadOnlyList<CertificateResponseDto>>(
                    "Student not found.");

            var certificates = await _uow.Certificates.GetByStudentIdAsync(studentId, ct);

            
            var result = certificates.Select(MapToDto).ToList();
            return _responseHandler.Success<IReadOnlyList<CertificateResponseDto>>(result);
        }

       
        public async Task<Response<IReadOnlyList<CertificateResponseDto>>> GetByCourseIdAsync(
            int courseId, CancellationToken ct = default)
        {
            
            var courseExists = await _uow.Courses.AnyAsync(c => c.Id == courseId, ct);
            if (!courseExists)
                return _responseHandler.NotFound<IReadOnlyList<CertificateResponseDto>>(
                    "Course not found.");

            var certificates = await _uow.Certificates.GetByCourseIdAsync(courseId, ct);

            var result = certificates.Select(MapToDto).ToList();
            return _responseHandler.Success<IReadOnlyList<CertificateResponseDto>>(result);
        }

        
        public async Task<Response<CertificateResponseDto>> UpdateAsync(
            int id, UpdateCertificateDto dto, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<CertificateResponseDto>(
                    $"Certificate with ID {id} was not found.");

           
            if (!string.IsNullOrWhiteSpace(dto.CertificateCode))
                certificate.CertificateCode = dto.CertificateCode;

            if (dto.IssuedAt != default)
                certificate.IssuedAt = dto.IssuedAt;

            if (!string.IsNullOrWhiteSpace(dto.FileUrl))
                certificate.FileUrl = dto.FileUrl;

            
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Success(MapToDto(certificate));
        }


        public async Task<Response<Certificate>> CreateAsync( CreateCertificateDto dto, CancellationToken ct = default)
        {
            
            var studentExists = await _uow.AppUserRepository.AnyAsync(u => u.Id == dto.StudentId, ct);
            if (!studentExists)
                return _responseHandler.NotFound<Certificate>("Student not found.");

            
            var courseExists = await _uow.Courses.AnyAsync(c => c.Id == dto.CourseId, ct);
            if (!courseExists)
                return _responseHandler.NotFound<Certificate>("Course not found.");

            
            var existingCertificates = await _uow.Certificates
                .GetByStudentIdAsync(dto.StudentId, ct);

            bool alreadyIssued = existingCertificates
                .Any(c => c.CourseId == dto.CourseId);

            if (alreadyIssued)
                return _responseHandler.BadRequest<Certificate>(
                    "A certificate has already been issued for this student in this course.");

            
            var certificate = new Certificate
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                CertificateCode = dto.CertificateCode,
                IssuedAt = dto.IssuedAt,
                FileUrl = dto.FileUrl
            };

            await _uow.Certificates.AddAsync(certificate, ct);
            await _uow.SaveChangesAsync(ct);

            
            var created = await _uow.Certificates.GetByIdAsync(certificate.Id, ct);
            return _responseHandler.Created(created!);
        }
        // ─────────────────────────────────────────────────────
        // DELETE /api/certificates/{id}
        // ─────────────────────────────────────────────────────
        public async Task<Response<string>> DeleteAsync(
            int id, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<string>(
                    $"Certificate with ID {id} was not found.");

            _uow.Certificates.Delete(certificate);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>();
        }

        // ─────────────────────────────────────────────────────
        // Private mapper — entity → DTO
        // ─────────────────────────────────────────────────────
        private static CertificateResponseDto MapToDto(Certificate c) => new()
        {
            Id = c.Id,
            StudentId = c.StudentId,
            CourseId = c.CourseId,
            CertificateCode = c.CertificateCode,
            IssuedAt = c.IssuedAt,
            FileUrl = c.FileUrl,
        };
    }
}
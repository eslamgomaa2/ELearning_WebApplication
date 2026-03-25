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

        public async Task<Response<Certificate>> GetByIdAsync(
            int id, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<Certificate>(
                    $"Certificate with ID {id} was not found.");

            return _responseHandler.Success(certificate);
        }

        
        public async Task<Response<IReadOnlyList<Certificate>>> GetByStudentIdAsync(
            Guid studentId, CancellationToken ct = default)
        {
            
            var studentExists = await _uow.AppUserRepository.AnyAsync(u => u.Id == studentId, ct);
            if (!studentExists)
                return _responseHandler.NotFound<IReadOnlyList<Certificate>>(
                    "Student not found.");

            var certificates = await _uow.Certificates.GetByStudentIdAsync(studentId, ct);

            
            
            return _responseHandler.Success<IReadOnlyList<Certificate>>(certificates);
        }

       
        public async Task<Response<IReadOnlyList<Certificate>>> GetByCourseIdAsync( int courseId,PaginationParams paginationParams, CancellationToken ct = default)
        {
            
            var courseExists = await _uow.Courses.AnyAsync(c => c.Id == courseId, ct);
            if (!courseExists)
                return _responseHandler.NotFound<IReadOnlyList<Certificate>>(
                    "Course not found.");

            var certificates = await _uow.Certificates.GetByCourseIdAsync(courseId, paginationParams,ct);

            
            return _responseHandler.Success<IReadOnlyList<Certificate>>(certificates);
        }

        
        public async Task<Response<Certificate>> UpdateAsync(
            int id, UpdateCertificateDto dto, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<Certificate>(
                    $"Certificate with ID {id} was not found.");

           
            if (!string.IsNullOrWhiteSpace(dto.CertificateCode))
                certificate.CertificateCode = dto.CertificateCode;

            if (dto.IssuedAt != default)
                certificate.IssuedAt = dto.IssuedAt;

            if (!string.IsNullOrWhiteSpace(dto.FileUrl))
                certificate.FileUrl = dto.FileUrl;

            
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Success(certificate);
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
        public async Task<Response<Certificate>> DeleteAsync(
            int id, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<Certificate>(
                    $"Certificate with ID {id} was not found.");

            _uow.Certificates.Delete(certificate);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<Certificate>();
        }

        
    }
}
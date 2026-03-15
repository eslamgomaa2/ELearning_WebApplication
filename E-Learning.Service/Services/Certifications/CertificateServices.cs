using AutoMapper;
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
        private readonly IMapper _mapper;

        public CertificateServices(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }


        public async Task<Response<Certificate>> GetByIdAsync(
            int id, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<Certificate>(
                    $"Certificate with ID {id} was not found.");

            return _responseHandler.Success(_mapper.Map<Certificate>(certificate));
        }


        public async Task<Response<IReadOnlyList<Certificate>>> GetByStudentIdAsync(
            Guid studentId, CancellationToken ct = default)
        {
            var certificates = await _uow.Certificates.GetByStudentIdAsync(studentId, ct);

            var result = _mapper.Map<IReadOnlyList<Certificate>>(certificates);
            return _responseHandler.Success(result);
        }


        public async Task<Response<IReadOnlyList<Certificate>>> GetByCourseIdAsync(
            int courseId, CancellationToken ct = default)
        {
            var certificates = await _uow.Certificates.GetByCourseIdAsync(courseId, ct);

            var result = _mapper.Map<IReadOnlyList<Certificate>>(certificates);
            return _responseHandler.Success(result);
        }


        public async Task<Response<Certificate>> CreateAsync(
            CreateCertificateDto dto, CancellationToken ct = default)
        {
                var existingCertificates = await _uow.Certificates.GetByStudentIdAsync(dto.StudentId, ct);
            bool alreadyIssued = existingCertificates.Any(c => c.CourseId == dto.CourseId);
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
            return _responseHandler.Created(_mapper.Map<Certificate>(created!));
        }


        public async Task<Response<Certificate>> UpdateAsync(
            int id, UpdateCertificateDto dto, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(id, ct);

            if (certificate is null)
                return _responseHandler.NotFound<Certificate>(
                    $"Certificate with ID {id} was not found.");

            certificate.CertificateCode = dto.CertificateCode;
            certificate.IssuedAt = dto.IssuedAt;
            certificate.FileUrl = dto.FileUrl;

            _uow.Certificates.Update(certificate);
            await _uow.SaveChangesAsync(ct);

            var updated = await _uow.Certificates.GetByIdAsync(id, ct);
            return _responseHandler.Success(_mapper.Map<Certificate>(updated!));
        }


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
    }
}
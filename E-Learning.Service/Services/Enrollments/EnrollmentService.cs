using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Core.Interfaces.Services.Enrollments;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Enrollments.Enrollment;

namespace E_Learning.Service.Services.Enrollments
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public EnrollmentService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }


        public async Task<Response<EnrollmentResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(id, ct);

            if (enrollment is null)
                return _responseHandler.NotFound<EnrollmentResponseDto>($"Enrollment with ID {id} was not found.");

            return _responseHandler.Success(_mapper.Map<EnrollmentResponseDto>(enrollment));
        }


        public async Task<Response<IReadOnlyList<EnrollmentResponseDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var enrollments = await _uow.Enrollments.GetAllAsync(ct);

            var result = _mapper.Map<IReadOnlyList<EnrollmentResponseDto>>(enrollments);
            return _responseHandler.Success(result);
        }


        public async Task<Response<IReadOnlyList<EnrollmentResponseDto>>> GetByStudentIdAsync(
            Guid studentId, CancellationToken ct = default)
        {
            var enrollments = await _uow.Enrollments.GetByStudentIdAsync(studentId, ct);

            var result = _mapper.Map<IReadOnlyList<EnrollmentResponseDto>>(enrollments);
            return _responseHandler.Success(result);
        }


        public async Task<Response<IReadOnlyList<EnrollmentResponseDto>>> GetByCourseIdAsync(
            int courseId, CancellationToken ct = default)
        {
            var enrollments = await _uow.Enrollments.GetByCourseIdAsync(courseId, ct);

            var result = _mapper.Map<IReadOnlyList<EnrollmentResponseDto>>(enrollments);
            return _responseHandler.Success(result);
        }


        public async Task<Response<EnrollmentResponseDto>> CreateAsync(
            CreateEnrollmentDto dto, CancellationToken ct = default)
        {
            bool alreadyEnrolled = await _uow.Enrollments.ExistsAsync(dto.StudentId, dto.CourseId, ct);
            if (alreadyEnrolled)
                return _responseHandler.BadRequest<EnrollmentResponseDto>(
                    "Student is already enrolled in this course.");

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                TransactionId = dto.TransactionId,
                EnrolledAt = DateTime.UtcNow
            };

            await _uow.Enrollments.AddAsync(enrollment, ct);
            await _uow.SaveChangesAsync(ct);

            var created = await _uow.Enrollments.GetByIdAsync(enrollment.Id, ct);
            return _responseHandler.Created(_mapper.Map<EnrollmentResponseDto>(created!));
        }


        public async Task<Response<EnrollmentResponseDto>> UpdateAsync(
            int id, UpdateEnrollmentDto dto, CancellationToken ct = default)
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(id, ct);

            if (enrollment is null)
                return _responseHandler.NotFound<EnrollmentResponseDto>(
                    $"Enrollment with ID {id} was not found.");

            if (dto.Status.HasValue)
                enrollment.Status = dto.Status.Value;

            if (dto.ProgressPercentage.HasValue)
            {
                if (dto.ProgressPercentage.Value < 0 || dto.ProgressPercentage.Value > 100)
                    return _responseHandler.BadRequest<EnrollmentResponseDto>(
                        "Progress percentage must be between 0 and 100.");

                enrollment.ProgressPercentage = dto.ProgressPercentage.Value;
            }

            if (dto.CompletedAt.HasValue)
                enrollment.CompletedAt = dto.CompletedAt.Value;

            _uow.Enrollments.Update(enrollment);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Success(_mapper.Map<EnrollmentResponseDto>(enrollment));
        }


        public async Task<Response<string>> SoftDeleteAsync(
            int id, string deletedBy, CancellationToken ct = default)
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(id, ct);

            if (enrollment is null)
                return _responseHandler.NotFound<string>($"Enrollment with ID {id} was not found.");

            _uow.Enrollments.SoftDelete(enrollment, deletedBy);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>();
        }
    }
}
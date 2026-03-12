using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Services.Enrollments;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Enrollments.LessonProgress;

namespace E_Learning.Service.Services.Enrollments
{
    public class LessonProgressService : ILessonProgressService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public LessonProgressService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }


        public async Task<Response<LessonProgressResponseDto>> GetByIdAsync(
            int id, CancellationToken ct = default)
        {
            var progress = await _uow.LessonProgresses.GetByIdAsync(id, ct);

            if (progress is null)
                return _responseHandler.NotFound<LessonProgressResponseDto>(
                    $"Lesson progress record with ID {id} was not found.");

            return _responseHandler.Success(_mapper.Map<LessonProgressResponseDto>(progress));
        }


        public async Task<Response<IReadOnlyList<LessonProgressResponseDto>>> GetByEnrollmentIdAsync(
            int enrollmentId, CancellationToken ct = default)
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(enrollmentId, ct);
            if (enrollment is null)
                return _responseHandler.NotFound<IReadOnlyList<LessonProgressResponseDto>>(
                    $"Enrollment with ID {enrollmentId} was not found.");

            var progresses = await _uow.LessonProgresses.GetByEnrollmentIdAsync(enrollmentId, ct);

            var result = _mapper.Map<IReadOnlyList<LessonProgressResponseDto>>(progresses);
            return _responseHandler.Success(result);
        }


        public async Task<Response<LessonProgressResponseDto>> GetByEnrollmentAndLessonAsync(
            int enrollmentId, int lessonId, CancellationToken ct = default)
        {
            var progress = await _uow.LessonProgresses
                .GetByEnrollmentAndLessonAsync(enrollmentId, lessonId, ct);

            if (progress is null)
                return _responseHandler.NotFound<LessonProgressResponseDto>(
                    $"No progress record found for enrollment {enrollmentId} and lesson {lessonId}.");

            return _responseHandler.Success(_mapper.Map<LessonProgressResponseDto>(progress));
        }


        public async Task<Response<LessonProgressResponseDto>> CreateAsync(
            CreateLessonProgressDto dto, CancellationToken ct = default)
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(dto.EnrollmentId, ct);
            if (enrollment is null)
                return _responseHandler.NotFound<LessonProgressResponseDto>(
                    $"Enrollment with ID {dto.EnrollmentId} was not found.");

            bool alreadyExists = await _uow.LessonProgresses
                .ExistsAsync(dto.EnrollmentId, dto.LessonId, ct);
            if (alreadyExists)
                return _responseHandler.BadRequest<LessonProgressResponseDto>(
                    "A progress record for this lesson already exists in the given enrollment.");

            var lessonProgress = new LessonProgress
            {
                EnrollmentId = dto.EnrollmentId,
                LessonId = dto.LessonId,
                WatchedSeconds = dto.WatchedSeconds,
                LastAccessedAt = DateTime.UtcNow
            };

            await _uow.LessonProgresses.AddAsync(lessonProgress, ct);
            await _uow.SaveChangesAsync(ct);

            var created = await _uow.LessonProgresses.GetByIdAsync(lessonProgress.Id, ct);
            return _responseHandler.Created(_mapper.Map<LessonProgressResponseDto>(created!));
        }


        public async Task<Response<LessonProgressResponseDto>> UpdateAsync(
            int id, UpdateLessonProgressDto dto, CancellationToken ct = default)
        {
            var progress = await _uow.LessonProgresses.GetByIdAsync(id, ct);

            if (progress is null)
                return _responseHandler.NotFound<LessonProgressResponseDto>(
                    $"Lesson progress record with ID {id} was not found.");

            if (dto.Status.HasValue)
                progress.Status = dto.Status.Value;

            if (dto.WatchedSeconds.HasValue)
            {
                if (dto.WatchedSeconds.Value < 0)
                    return _responseHandler.BadRequest<LessonProgressResponseDto>(
                        "WatchedSeconds cannot be negative.");

                progress.WatchedSeconds = dto.WatchedSeconds.Value;
            }

            if (dto.CompletedAt.HasValue)
            {
                progress.CompletedAt = dto.CompletedAt.Value;
                progress.Status = LessonProgressStatus.Completed;
            }

            progress.LastAccessedAt = DateTime.UtcNow;

            _uow.LessonProgresses.Update(progress);
            await _uow.SaveChangesAsync(ct);

            var updated = await _uow.LessonProgresses.GetByIdAsync(id, ct);
            return _responseHandler.Success(_mapper.Map<LessonProgressResponseDto>(updated!));
        }


        public async Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var progress = await _uow.LessonProgresses.GetByIdAsync(id, ct);

            if (progress is null)
                return _responseHandler.NotFound<string>(
                    $"Lesson progress record with ID {id} was not found.");

            _uow.LessonProgresses.Delete(progress);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>();
        }
    }
}
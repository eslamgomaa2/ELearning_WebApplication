using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Enrollments.LessonProgress;

namespace E_Learning.Core.Interfaces.Services.Enrollments
{
    public interface ILessonProgressService
    {
        Task<Response<LessonProgressResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Response<IReadOnlyList<LessonProgressResponseDto>>> GetByEnrollmentIdAsync(int enrollmentId, CancellationToken ct = default);
        Task<Response<LessonProgressResponseDto>> GetByEnrollmentAndLessonAsync(int enrollmentId, int lessonId, CancellationToken ct = default);
        Task<Response<LessonProgressResponseDto>> CreateAsync(CreateLessonProgressDto dto, CancellationToken ct = default);
        Task<Response<LessonProgressResponseDto>> UpdateAsync(int id, UpdateLessonProgressDto dto, CancellationToken ct = default);
        Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
using E_Learning.Core.Entities.Enrollment;

namespace E_Learning.Core.Interfaces.Repositories.Enrollments
{
    public interface ILessonProgressRepository
    {

        Task<LessonProgress?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<IReadOnlyList<LessonProgress>> GetByEnrollmentIdAsync(int enrollmentId, CancellationToken ct = default);

        Task<LessonProgress?> GetByEnrollmentAndLessonAsync(int enrollmentId, int lessonId, CancellationToken ct = default);

        Task<bool> ExistsAsync(int enrollmentId, int lessonId, CancellationToken ct = default);


        Task AddAsync(LessonProgress lessonProgress, CancellationToken ct = default);
        void Update(LessonProgress lessonProgress);
        void Delete(LessonProgress lessonProgress);
    }
}
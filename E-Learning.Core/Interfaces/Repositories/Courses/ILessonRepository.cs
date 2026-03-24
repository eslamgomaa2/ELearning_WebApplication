using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.core.Interfaces.Repositories.Courses
{
    public interface ILessonRepository : IGenericRepository<Lesson,int>
    {
        Task<IReadOnlyList<Lesson>> GetLessonsBySectionIdAsync(int sectionId, CancellationToken ct = default);

        Task<IReadOnlyList<Lesson>> GetLessonsByCourseIdAsync(int courseId, CancellationToken ct = default);
    }
}
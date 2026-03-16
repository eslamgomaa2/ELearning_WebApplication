using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.core.Interfaces.Repositories.Courses
{
    public interface ISectionRepository : IGenericRepository<Section,int>
    {
        Task<IReadOnlyList<Section>> GetSectionsByCourseIdAsync(int courseId, CancellationToken ct = default);
    }
}
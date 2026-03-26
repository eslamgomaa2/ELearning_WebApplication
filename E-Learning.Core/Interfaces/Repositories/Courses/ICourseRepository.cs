using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Specifications;

namespace E_Learning.core.Interfaces.Repositories.Courses
{
    public interface ICourseRepository : IGenericRepository<Course,int>
    {
        Task<IReadOnlyList<Course>> GetAllWithSpecAsync(ISpecifications<Course> Spec, CancellationToken ct = default);
        Task<int> CountAsync(ISpecifications<Course> spec, CancellationToken ct = default);
    }
}
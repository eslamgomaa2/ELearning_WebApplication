using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.core.Interfaces.Repositories.Courses
{
    public interface ICourseRepository : IGenericRepository<Course,int>
    {
    }
}
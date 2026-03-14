using E_Learning.core.Interfaces.Repositories.Courses;
using E_Learning.Core.Entities.Courses;
using E_Learning.Repository.Data;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Courses
{
    public class CourseRepository: GenericRepository<Course,int>,ICourseRepository
    {
        public CourseRepository(ELearningDbContext context): base(context) 
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

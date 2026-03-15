using E_Learning.core.Interfaces.Repositories.Courses;
using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Specifications;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Courses
{
    public class CourseRepository: GenericRepository<Course,int>,ICourseRepository
    {
        public CourseRepository(ELearningDbContext context): base(context) 
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }

        public async Task<IReadOnlyList<Course>> GetAllWithSpecAsync(
        ISpecifications<Course> spec,
        CancellationToken ct = default)
        {
            IQueryable<Course> query = _context.Set<Course>();

            // WHERE
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            // Includes
            foreach (var include in spec.Includes)
                query = query.Include(include);

            foreach (var includeString in spec.IncludeStrings)
                query = query.Include(includeString);

            // Sorting
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            // Pagination
            if (spec.IsPaginated)
                query = query.Skip(spec.Skip).Take(spec.Take);

            // NoTracking
            if (spec.AsNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(ct);
        }
    }
}

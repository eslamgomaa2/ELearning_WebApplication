using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Features.Courses.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Specifications.Courses
{
    public class CourseSpecification:BaseSpecification<Course>
    {
        public CourseSpecification(CourseQuery query):base
            (x=>
                (!query.InstructorId.HasValue || x.InstructorId == query.InstructorId) &&
                (!query.StudentId.HasValue || x.Enrollments.Any(e => e.StudentId == query.StudentId)) &&
                (!query.LevelId.HasValue || x.LevelId == query.LevelId) &&
                (string.IsNullOrEmpty(query.Subject) || x.Title.Contains(query.Subject)) &&
                (string.IsNullOrEmpty(query.StageType) || x.Level!.Stage.Name == query.StageType) &&
                (string.IsNullOrEmpty(query.Status) || x.Status == query.Status) &&
                (string.IsNullOrEmpty(query.Search) || x.Title.Contains(query.Search) || x.Enrollments.Any(x=>x.StudentId.ToString() == query.Search))
            )
        {
            AddInclude(x => x.Sections);
            AddInclude(x => x.Enrollments);
            AddInclude("Enrollments.Student");
            AddInclude(x => x.Level);
            AddInclude(x => x.Instructor);

            // Sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "title":
                        if (query.Desc) AddOrderByDescending(x => x.Title);
                        else AddOrderBy(x => x.Title);
                        break;
                    case "price":
                        if (query.Desc) AddOrderByDescending(x => x.Price);
                        else AddOrderBy(x => x.Price);
                        break;
                    case "duration":
                        if (query.Desc) AddOrderByDescending(x => x.DurationInMinutes);
                        else AddOrderBy(x => x.DurationInMinutes);
                        break;
                }
            }

            // Pagination
            ApplyPagination(query.PageNumber, query.PageSize);

            ApplyNoTracking();
        }
    }
}

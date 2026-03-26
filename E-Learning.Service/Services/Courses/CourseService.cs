using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Features.Courses.Queries;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Core.Repository;
using E_Learning.Core.Specifications.Courses;
using E_Learning.Repository.Repositories;
using E_Learning.Service.DTOs.Course;
using E_Learning.Service.DTOs.CourseDto;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Learning.Service.Services.Courses
{
    public class CourseService : ICourseService
    {        
        private readonly IMapper _mapper;
        private  ResponseHandler _response;
        private readonly IUnitOfWork _unit;

        public CourseService( 
            IMapper mapper,
            ResponseHandler response,
            IUnitOfWork unit
            )
        {            
            _mapper = mapper;
            _response = response;
            _unit = unit;
        }

        public async Task<Response<CourseDto>> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default)
        {

            var instructor = await _unit.AppUserRepository.GetByIdAsync(dto.InstructorId);
            
            if (instructor == null)
                return _response.NotFound<CourseDto>("Instructor not found");

            var course = _mapper.Map<Course>(dto);

            course.Slug = $"{dto.Title.Trim().ToLower().Replace(" ", "-")}-{Guid.NewGuid().ToString()[..6]}";
            
            await _unit.Courses.AddAsync(course);

            await _unit.SaveChangesAsync();
            var result = _mapper.Map<CourseDto>(course);
            
            return _response.Created(result);
        }

        public async Task<Response<string>> DeleteCourseAsync(int id, CancellationToken ct = default)
        {
            var Course = await _unit.Courses.GetByIdAsync(id);
                
            if (Course == null)
                return _response.NotFound<string>("Course not found");
            _unit.Courses.Remove(Course);
            await _unit.SaveChangesAsync();
            return _response.Deleted<string>();
        }

        public async Task<Response<CourseDto>> GetCourseByIdAsync(int id, CancellationToken ct = default)
        {
            var Course = await _unit.Courses.GetByIdAsync(id
                , x => x.Include(x => x.Sections)
                    .ThenInclude(x => x.Lessons));
            if (Course == null) 
                return _response.NotFound<CourseDto>("Course not found");
            var dto = _mapper.Map<CourseDto>(Course);
            return _response.Success(dto);
        }

        public async Task<Response<IReadOnlyList<CourseDto>>> GetCoursesAsync(CancellationToken ct = default)
        {

            var courses = await _unit.Courses.GetAllAsync();

            if (courses == null)
                return _response.NotFound<IReadOnlyList<CourseDto>>("Course not found");

            var Result = _mapper.Map<IReadOnlyList<CourseDto>>(courses);
            return _response.Success(Result);
        }

        public async Task<Response<IReadOnlyList<CourseDto>>> GetCoursesAsync(CourseQuery query,
            CancellationToken ct = default)
        {
            var spec = new CourseSpecification(query);

            var courses = await _unit.Courses.GetAllWithSpecAsync(spec, ct);

            if (courses == null || !courses.Any())
                return _response.NotFound<IReadOnlyList<CourseDto>>("No courses found");

            var result = _mapper.Map<IReadOnlyList<CourseDto>>(courses);

            return _response.Success(result);
        }

        public async Task<Response<string>> UpdateCourseAsync(int id,UpdateCourseDto dto, CancellationToken ct = default)
        {
            var Course =await _unit.Courses.GetByIdAsync(id);
            if (Course == null)
                return _response.NotFound<string>("Course not found");
            
            _mapper.Map(dto, Course);
            _unit.Courses.Update(Course);
            await _unit.SaveChangesAsync();
            return _response.Success("Updated Successfully");
        }
    }
}

using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Repository.Repositories;
using E_Learning.Service.DTOs.Course;
using E_Learning.Service.DTOs.CourseDto;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.Courses
{
    public class CourseService : ICourseService
    {
        
        private readonly IMapper _mapper;
        private  ResponseHandler _response;
        private readonly UnitOfWork _unit;

        public CourseService( 
            IMapper mapper,
            ResponseHandler response,
            UnitOfWork unit
            )
        {            
            _mapper = mapper;
            _response = response;
            _unit = unit;
        }

        public async Task<Response<CourseDto>> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default)
        {

            var instructor = await _unit.InstructorProfiles.GetByIdAsync(dto.InstructorId);
            
            if (instructor == null)
                return _response.NotFound<CourseDto>("Instructor not found");

            var course = _mapper.Map<Course>(dto);

            await _unit.Courses.AddAsync(course);
            var result = _mapper.Map<CourseDto>(course);
            await _unit.SaveChangesAsync();
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
            var Courses = await _unit.Courses.GetAllAsync();
            var Result = _mapper.Map<IReadOnlyList<CourseDto>>(Courses);
            return  _response.Success(Result);
        }

        public async Task<Response<string>> UpdateCourseAsync(UpdateCourseDto dto, CancellationToken ct = default)
        {
            var Course =await _unit.Courses.GetByIdAsync(dto.Id);
            if (Course == null)
                return _response.NotFound<string>("Course not found");
            
            _mapper.Map(dto, Course);
            _unit.Courses.Update(Course);
            await _unit.SaveChangesAsync();
            return _response.Success("Updated Successfully");
        }
    }
}

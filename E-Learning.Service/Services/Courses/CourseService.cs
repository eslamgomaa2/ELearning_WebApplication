using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Service.DTOs.Course;
using E_Learning.Service.DTOs.CourseDto;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course, int> _courseRepo;
        private readonly IMapper _mapper;
        private  ResponseHandler _response;

        public CourseService(
            IGenericRepository<Course,int> courseRepo,
            IMapper mapper,
            ResponseHandler response
            )
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
            _response = response;
        }

        public async Task<Response<CourseDto>> CreateCourseAsync(CreateCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            await _courseRepo.AddAsync(course);
            var result = _mapper.Map<CourseDto>(course);
            return _response.Created(result);
        }

        public async Task<Response<string>> DeleteCourseAsync(int id)
        {
            var Course = await _courseRepo.GetByIdAsync(id
                ,x=>x.Include(x=>x.Sections)
                    .ThenInclude(x=>x.Lessons));
            if (Course == null)
                return _response.NotFound<string>("Course not found");
            _courseRepo.Remove(Course);
            return _response.Deleted<string>();
        }

        public async Task<Response<CourseDto>> GetCourseByIdAsync(int id)
        {
            var Course = await _courseRepo.GetByIdAsync(
                id
            );
            if (Course == null) 
                return _response.NotFound<CourseDto>("Course not found");
            var dto = _mapper.Map<CourseDto>(Course);
            return _response.Success(dto);
        }

        public async Task<Response<IReadOnlyList<CourseDto>>> GetCoursesAsync()
        {
            var Courses = await _courseRepo.GetAllAsync();
            var Result = _mapper.Map<IReadOnlyList<CourseDto>>(Courses);
            return  _response.Success(Result);
        }

        public async Task<Response<string>> UpdateCourseAsync(UpdateCourseDto dto)
        {
            var Course =await _courseRepo.GetByIdAsync(dto.Id);
            if (Course == null)
                return _response.NotFound<string>("Course not found");
            _mapper.Map(dto, Course);
            _courseRepo.Update(Course);
            return _response.Success("Updated Successfully");
        }
    }
}

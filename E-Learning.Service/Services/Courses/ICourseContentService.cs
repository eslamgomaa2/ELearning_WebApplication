using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Lesson;
using E_Learning.Service.DTOs.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Courses
{
    public interface ICourseContentService
    {
        Task<Response<SectionDto>> CreateSectionAsync(int courseId, CreateSectionDto dto, CancellationToken ct = default);

        Task<Response<SectionDto>> UpdateSectionAsync(int sectionId, UpdateSectionDto dto, CancellationToken ct = default);

        Task<Response<string>> DeleteSectionAsync(int sectionId, CancellationToken ct = default);

        Task<Response<IReadOnlyList<SectionDto>>> GetSectionsByCourseIdAsync(int courseId, CancellationToken ct = default);


        Task<Response<LessonDto>> CreateLessonAsync(int sectionId, CreateLessonDto dto, CancellationToken ct = default);

        Task<Response<LessonDto>> UpdateLessonAsync(int lessonId, UpdateLessonDto dto, CancellationToken ct = default);

        Task<Response<string>> DeleteLessonAsync(int lessonId, CancellationToken ct = default);

        Task<Response<IReadOnlyList<LessonDto>>> GetLessonsBySectionIdAsync(int sectionId, CancellationToken ct = default);

        Task<Response<IReadOnlyList<LessonDto>>> GetLessonsByCourseIdAsync(int courseId, CancellationToken ct = default);
    }
}

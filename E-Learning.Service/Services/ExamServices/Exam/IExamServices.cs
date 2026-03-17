using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.ExamServices.Exam
{
    public interface IExamServices
    {
        public Task<Response<E_Learning.Core.Entities.Assessments.Exams.Exam>> CreateAsync(CreateExamDto createExamDto, CancellationToken ct);
        public Task<Response<E_Learning.Core.Entities.Assessments.Exams.Exam>> GetByIdAsync(int id);
        Task<Response<string>> UpdateQuestionOrderAsync(int examId, UpdateQuestionOrderDto dto, CancellationToken ct);

    }
}

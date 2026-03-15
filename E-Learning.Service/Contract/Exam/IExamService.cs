using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.Service.DTOs.AssignmentsDto;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.ExamsDtos;
using E_Learning.Core.Base;

namespace E_Learning.Service.Contract.Exam
{
    public interface IExamService
    {
        Task<Response<PagedResultDto<ExamDto>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Response<ExamDto?>> GetByIdAsync(int id);
        Task<Response<ExamDto>> CreateAsync(CreateExamDto dto);
        Task<Response<ExamDto?>> UpdateAsync(int id,UpdateExamDto dto);
        Task<Response<string>> DeleteAsync(int id);

    }
}

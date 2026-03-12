using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.Core.Base;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.AssignmentsDto;

namespace E_Learning.Service.Contract.Assignments
{
    public interface IAssignmentService
    {
   

        Task<Response<AssignmentDto>> CreateAsync(CreateAssignmentDto dto);

        Task<Response<AssignmentDto>> UpdateAsync(int id, UpdateAssignmentDto dto);

        Task<Response<string>> DeleteAsync(int id);

        Task<Response<AssignmentDto>> GetByIdAsync(int id);

         Task<Response<PagedResultDto<AssignmentDto>>> GetAllAsync(int pageNumber, int pageSize);
    }
}

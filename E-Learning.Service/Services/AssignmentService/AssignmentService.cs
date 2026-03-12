using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Assessments.Assignments;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract.Assignments;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.AssignmentsDto;

namespace E_Learning.Service.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork  _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _response;

        public AssignmentService(IUnitOfWork unitOfWork  ,  IMapper mapper, ResponseHandler response)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._response = response;
        }

        public async Task<Response<AssignmentDto>> CreateAsync(CreateAssignmentDto dto)
        {
            var assignment = _mapper.Map<Assignment>(dto);

            await _unitOfWork.Assignments.AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<AssignmentDto>(assignment);


            return _response.Created<AssignmentDto>(result);
        }

        public async Task<Response<AssignmentDto>> UpdateAsync(int id, UpdateAssignmentDto dto)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);

            if (assignment == null)
                return _response.NotFound<AssignmentDto>("Assignment not found");

            _mapper.Map(dto, assignment);

            _unitOfWork.Assignments.Update(assignment);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<AssignmentDto>(assignment);

            return _response.Success(result);
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);

            if (assignment == null)
                return _response.NotFound<string>("Assignment not found");

            _unitOfWork.Assignments.Remove(assignment);
            await _unitOfWork.SaveChangesAsync();

            return _response.Deleted<string>("Assignment deleted successfully");
        }
        public async Task<Response<AssignmentDto>> GetByIdAsync(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);

            if (assignment == null)
                return _response.NotFound<AssignmentDto>("Assignment not found");

            var result = _mapper.Map<AssignmentDto>(assignment);

            return _response.Success(result);
        }
        public async Task<Response<PagedResultDto<AssignmentDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
           
            var (assignments, totalCount) = await _unitOfWork.Assignments.GetPagedAsync(pageNumber, pageSize);

           
            var assignmentDtos = _mapper.Map<IReadOnlyList<AssignmentDto>>(assignments);

          
            var pagedResult = new PagedResultDto<AssignmentDto>
            {
                Items = assignmentDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

         
            return _response.Success(pagedResult);
        }
    }
}

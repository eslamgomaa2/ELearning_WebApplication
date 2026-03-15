using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract;
using E_Learning.Service.Contract.Exam;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.ExamsDtos;

namespace E_Learning.Service.Services.ExamService
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _response;
        private readonly IFileService _fileService;

        public ExamService(IUnitOfWork unitOfWork, IMapper mapper, ResponseHandler response, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._response = response;
            this._fileService = fileService;
        }

        public async Task<Response<ExamDto>> CreateAsync(CreateExamDto dto)
        {
            string fileUrl = null;

            if (dto.File != null)
            {
                fileUrl = await _fileService.UploadFileAsync<object>(dto.File, "exams");
            }

            var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);

            if (course == null)
                return _response.NotFound<ExamDto>("Course not found");

            var exam = _mapper.Map<Exam>(dto);

            exam.SourceFileUrl = fileUrl;

            await _unitOfWork.Exams.AddAsync(exam);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ExamDto>(exam);

            return _response.Created(result);
        }

        public async Task< Response<string>> DeleteAsync(int id)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam == null)
                return _response.NotFound<string>("Exam not found with this id");

            _unitOfWork.Exams.Remove(exam);
            await _unitOfWork.SaveChangesAsync();

            return _response.Deleted<string>("Exam deleted successfully");
        }

        public async Task< Response<PagedResultDto<ExamDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (exams, totalCount) = await _unitOfWork.Exams.GetPagedAsync(pageNumber, pageSize);

            var examDtos = _mapper.Map<IReadOnlyList<ExamDto>>(exams);

            var pagedResult = new PagedResultDto<ExamDto>
            {
                Items = examDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            if (totalCount == 0)
            {
                return _response.NotFound<PagedResultDto<ExamDto>>("No exams found");
            }
            return _response.Success(pagedResult);
        }

        public async Task< Response<ExamDto?>> GetByIdAsync(int id)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam == null)
                return _response.NotFound<ExamDto>("Exam not found with this id");

            var result = _mapper.Map<ExamDto>(exam);
            return _response.Success(result);
        }

        public async Task< Response<ExamDto?>> UpdateAsync(int id, UpdateExamDto dto)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam == null)
                return _response.NotFound<ExamDto>("Exam not found");
            string fileUrl = null;

            if (dto.File != null)
            {
                fileUrl = await _fileService.UploadFileAsync<object>(dto.File, "exams");
            }

            _mapper.Map(dto, exam);
            exam.SourceFileUrl = fileUrl;
            _unitOfWork.Exams.Update(exam);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ExamDto>(exam);
            return _response.Success(result);
        }
    }
}

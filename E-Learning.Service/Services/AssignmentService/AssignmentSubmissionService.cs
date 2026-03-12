using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract;
using E_Learning.Service.Contract.Assignments;
using E_Learning.Service.DTOs.AssignmentsDto;

namespace E_Learning.Service.Services.AssignmentService
{
    
        public class AssignmentSubmissionService : IAssignmentSubmissionService
        { 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;
        private readonly IFileService _fileService;

        public AssignmentSubmissionService(
                IUnitOfWork unitOfWork,
                IMapper mapper,ResponseHandler responseHandler, IFileService fileService)
            {
                 
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
            this._responseHandler = responseHandler;
            _fileService = fileService;
        }

            public async Task<Response<AssignmentSubmissionDto>> CreateSubmitAsync(CreateSubmissionDto dto)
            {
                    string fileUrl = null;

                    if (dto.File != null)
                    {
                        fileUrl = await _fileService.UploadFileAsync<object>(dto.File, "submissions");
                    }
                 var assignment = await  _unitOfWork.Assignments.GetByIdAsync(dto.AssignmentId);

                if (assignment == null)
                   return  _responseHandler.BadRequest<AssignmentSubmissionDto>("Assignment not found");

                if (assignment.DueDate < DateTime.UtcNow)
                  return  _responseHandler.BadRequest<AssignmentSubmissionDto>("Submission deadline has passed");
 
                    var submission = _mapper.Map<AssignmentSubmission>(dto);

                submission.SubmittedAt = DateTime.UtcNow;
                submission.Status = AssignmentStatus.Submitted;

                 submission.FileUrl=fileUrl;
                await _unitOfWork.AssignmentSubmissions.AddAsync(submission);
                await _unitOfWork.SaveChangesAsync();
               var result = _mapper.Map<AssignmentSubmissionDto>(submission);

                  return _responseHandler.Created<AssignmentSubmissionDto>(result);
            }

            public async Task<Response<AssignmentSubmissionDto>> CreateGradeAsync(int submissionId, GradeSubmissionDto dto)
            {
                var submission = await _unitOfWork.AssignmentSubmissions.GetByIdAsync(submissionId);

                if (submission == null)
                       return _responseHandler.NotFound<AssignmentSubmissionDto>("Submission not found");
 
                submission.Score = dto.Score;
                submission.TeacherComment = dto.TeacherComment;
                submission.Status = AssignmentStatus.Graded;

                 _unitOfWork.AssignmentSubmissions.Update(submission);
                   await _unitOfWork.SaveChangesAsync();
                var result=      await  _unitOfWork.AssignmentSubmissions.GetAssignmentSubmissionByIdWithAssimentData(submissionId);
          
                   var assm= _mapper.Map<AssignmentSubmissionDto>(result);
                  return _responseHandler.Created<AssignmentSubmissionDto>(assm);
            }

            public async Task<Response<IReadOnlyList<AssignmentSubmissionDto>>> GetAllByAssignmentAsync(int assignmentId)
            {
               var assignment=  await _unitOfWork.Assignments.GetByIdAsync(assignmentId);

            if (assignment == null)
               return _responseHandler.NotFound<IReadOnlyList<AssignmentSubmissionDto>>("Assignment not found ");

            var result = await _unitOfWork.AssignmentSubmissions.GetByAssignmentIdAsync(assignmentId);
                    if(!result.Any())
                       return  _responseHandler.NotFound<IReadOnlyList<AssignmentSubmissionDto>>("Submission not found for this assiment");
                  var r= _mapper.Map<IReadOnlyList<AssignmentSubmissionDto>>(result);
            
                  return _responseHandler.Success<IReadOnlyList<AssignmentSubmissionDto>>(r);
            }

            public async Task<Response<IReadOnlyList<AssignmentSubmissionDto>>> GetByStudentAsync(Guid studentId)
            {
                 var asb= await _unitOfWork.AssignmentSubmissions.GetByStudentIdAsync(studentId);
            if (!asb.Any())
                return _responseHandler.NotFound<IReadOnlyList<AssignmentSubmissionDto>>("Submission not found for this student");
            var r = _mapper.Map<IReadOnlyList<AssignmentSubmissionDto>>(asb);
            return _responseHandler.Success<IReadOnlyList<AssignmentSubmissionDto>>(r);



        }
    }
    
}

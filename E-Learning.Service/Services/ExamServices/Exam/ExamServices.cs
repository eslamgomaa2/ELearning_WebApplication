using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.ExamServices.Exam
{
    public class ExamServices : IExamServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;

        public ExamServices(IUnitOfWork unitOfWork, ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        public async Task<Response<Core.Entities.Assessments.Exams.Exam>> CreateAsync(CreateExamDto createExamDto, CancellationToken ct)
        {
            var courseExists = await _unitOfWork.Courses.AnyAsync(c => c.Id == createExamDto.CourseId, ct);
            if (!courseExists)
                return _responseHandler.NotFound<Core.Entities.Assessments.Exams.Exam>("Course not found.");

            var exam = new Core.Entities.Assessments.Exams.Exam
            {
                Title = createExamDto.Title,
                CourseId = createExamDto.CourseId,
                EducationLevel = createExamDto.EducationLevel,
                ScheduledAt = createExamDto.ScheduledAt,
                EndDateTime = createExamDto.EndDateTime,
                DurationSeconds = createExamDto.DurationMinutes * 60,
                Instructions = createExamDto.Instructions,
                Rules = createExamDto.Rules,
                TechnicalRequirements = createExamDto.TechnicalRequirements,
                TotalMarks = createExamDto.TotalMarks,
                PassingScore = createExamDto.PassingScore,
                MaxAttempts = createExamDto.MaxAttempts,
                AIShuffleEnabled = createExamDto.AIShuffleEnabled,
                
            };
            await _unitOfWork.Exams.AddAsync(exam, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return _responseHandler.Created<Core.Entities.Assessments.Exams.Exam>(exam);
        }

        public async Task<Response<Core.Entities.Assessments.Exams.Exam>> GetByIdAsync(int id)
        {
           var exam= await _unitOfWork.Exams.GetByIdWithQuestionsAsync(id);
            if(exam == null)
            {
                return _responseHandler.NotFound<Core.Entities.Assessments.Exams.Exam>($"Exam with id {id} not found.");
            }
            return _responseHandler.Success(exam);
        }

        public async Task<Response<string>> UpdateQuestionOrderAsync( int examId, UpdateQuestionOrderDto dto, CancellationToken ct)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(examId, ct);
            if (exam is null)
                return _responseHandler.NotFound<string>("Exam not found.");

            // Prevent changing order on a published exam
            if (exam.IsActive)
                return _responseHandler.BadRequest<string>(
                    "Cannot change question order on a published exam.");

            exam.AIShuffleEnabled = dto.AIShuffleEnabled;

            _unitOfWork.Exams.Update(exam);
            await _unitOfWork.SaveChangesAsync(ct);

            var message = dto.AIShuffleEnabled
                ? "AI Shuffle enabled. Each student will receive a unique question order."
                : "Original order kept. Questions will appear in the order they were created.";

            return _responseHandler.Success(message);
        }
    }
}

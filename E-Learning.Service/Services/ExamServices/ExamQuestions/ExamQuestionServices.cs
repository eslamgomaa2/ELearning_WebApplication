using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Repository;

namespace E_Learning.Service.Services.ExamServices.Questions
{
    public class ExamQuestionServices : IExamQuestionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;

        public ExamQuestionServices(IUnitOfWork unitOfWork, ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }


        public async Task<Response<IReadOnlyList<ExamQuestion>>> AddManuallyAsync(
     int examId, AddQuestionsDto dto, CancellationToken ct)
        {
            // 1. Exam must exist
            var examExists = await _unitOfWork.Exams.AnyAsync(e => e.Id == examId, ct);
            if (!examExists)
                return _responseHandler.NotFound<IReadOnlyList<ExamQuestion>>("Exam not found.");

            // 2. Start OrderIndex after existing questions
            var startIndex = await _unitOfWork.ExamQuestions.GetMaxOrderIndexAsync(examId, ct);

            var questions = dto.Questions
                .Select((q, i) => new ExamQuestion
                {
                    ExamId = examId,
                    Text = q.QuestionText,
                    Type = q.QuestionType,
                    Points = q.Points,
                    IsAIGenerated = false,
                    OrderIndex = q.Order > 0 ? q.Order : startIndex + i + 1,
                    Options = q.Options
                        .Select((o, oi) => new ExamOption
                        {
                            Text = o.OptionText,
                            IsCorrect = o.IsCorrect,
                            OrderIndex = oi + 1,
                        })
                        .ToList()
                })
                .ToList();

            await _unitOfWork.ExamQuestions.AddRangeAsync(questions, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Created<IReadOnlyList<ExamQuestion>>(questions);
        }


        public async Task<Response<IReadOnlyList<ExamQuestion>>> GetQuestionsByExamIdAsync(int examId,PaginationParams paginationParams ,CancellationToken ct)
        {
            var examExists = await _unitOfWork.Exams.AnyAsync(e => e.Id == examId, ct);
            if (!examExists)
                return _responseHandler.NotFound<IReadOnlyList<ExamQuestion>>("Exam not found.");


            var questions = await _unitOfWork.ExamQuestions.GetByExamIdAsync(examId, paginationParams,ct);

            

            return _responseHandler.Success<IReadOnlyList<ExamQuestion>>(questions);
        }


        public async Task<Response<ExamQuestion>> UpdateAsync(
     int examId, int questionId, UpdateQuestionDto dto, CancellationToken ct)
        {
            
            var question = await _unitOfWork.ExamQuestions
                                 .GetByIdWithOptionsAsync(questionId, ct);

            
            if (question is null || question.ExamId != examId)
                return _responseHandler.NotFound<ExamQuestion>("Question not found.");

            
            question.Text = dto.QuestionText;
            question.Type = dto.QuestionType;
            question.Points = dto.Points;
            question.OrderIndex = dto.Order;

            
            var incomingIds = dto.Options
                .Where(o => o.Id.HasValue)
                .Select(o => o.Id!.Value)
                .ToHashSet();

            var toRemove = question.Options
                .Where(o => !incomingIds.Contains(o.Id))
                .ToList();

            foreach (var opt in toRemove)
                question.Options.Remove(opt);

            for (int i = 0; i < dto.Options.Count; i++)
            {
                var optDto = dto.Options[i];
                if (optDto.Id.HasValue)
                {
                    var existing = question.Options
                        .FirstOrDefault(o => o.Id == optDto.Id.Value);
                    if (existing is not null)
                    {
                        existing.Text = optDto.OptionText;
                        existing.IsCorrect = optDto.IsCorrect;
                        existing.OrderIndex = i + 1;
                    }
                }
                else
                {
                    question.Options.Add(new ExamOption
                    {
                        Text = optDto.OptionText,
                        IsCorrect = optDto.IsCorrect,
                        OrderIndex = i + 1,
                    });
                }
            }

            
            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Success(question);
        }

        public async Task<Response<ExamQuestion>> DeleteAsync(
            int examId, int questionId, CancellationToken ct)
        {
            var exists = await _unitOfWork.ExamQuestions.ExistsAsync(examId, questionId, ct);
            if (!exists)
                return _responseHandler.NotFound<ExamQuestion>("Question not found.");

           var question= await _unitOfWork.ExamQuestions.GetByIdAsync(questionId, ct);
             _unitOfWork.ExamQuestions.Remove(question);
            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Deleted<ExamQuestion>();
        }

      
        public async Task<Response<string>> ReorderAsync(
            int examId, ReorderQuestionsDto dto, CancellationToken ct)
        {
            var questions = await _unitOfWork.ExamQuestions
                .GetByIdsAsync(examId, dto.QuestionIdsInOrder, ct);

            // All submitted IDs must belong to this exam
            if (questions.Count != dto.QuestionIdsInOrder.Count)
                return _responseHandler.BadRequest<string>(
                    "One or more question IDs are invalid for this exam.");

            // Assign new OrderIndex based on position in the submitted list
            for (int i = 0; i < dto.QuestionIdsInOrder.Count; i++)
            {
                var question = questions.First(q => q.Id == dto.QuestionIdsInOrder[i]);
                question.OrderIndex = i + 1;
                _unitOfWork.ExamQuestions.Update(question);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Success("Questions reordered successfully.");
        }

        
       
    }
}
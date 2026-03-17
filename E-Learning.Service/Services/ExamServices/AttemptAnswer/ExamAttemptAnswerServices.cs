using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Enums;
using E_Learning.Core.Repository;

namespace E_Learning.Service.Services.ExamServices.Answers
{
    public class ExamAttemptAnswerServices : IExamAttemptAnswerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;

        public ExamAttemptAnswerServices(
            IUnitOfWork unitOfWork,
            ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        // ─────────────────────────────────────────────────────────
        // GET /api/exams/{examId}/attempts/{attemptId}/answers
        // ─────────────────────────────────────────────────────────
        public async Task<Response<IReadOnlyList<AttemptAnswerResponseDto>>> GetByAttemptAsync(
            int examId, int attemptId, CancellationToken ct)
        {
            // Validate attempt belongs to this exam
            var attempt = await _unitOfWork.ExamAttempts.GetByIdAsync(attemptId, ct);
            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<IReadOnlyList<AttemptAnswerResponseDto>>(
                    "Attempt not found.");

            var answers = await _unitOfWork.ExamAttemptAnswers
                .GetByAttemptIdAsync(attemptId, ct);

            var result = answers.Select(MapToDto).ToList();
            return _responseHandler.Success<IReadOnlyList<AttemptAnswerResponseDto>>(result);
        }

        // ─────────────────────────────────────────────────────────
        // GET /api/exams/{examId}/attempts/{attemptId}/answers/{answerId}
        // ─────────────────────────────────────────────────────────
        public async Task<Response<AttemptAnswerResponseDto>> GetByIdAsync(
            int examId, int attemptId, int answerId, CancellationToken ct)
        {
            var answer = await _unitOfWork.ExamAttemptAnswers
                .GetByIdWithDetailsAsync(answerId, ct);

            if (answer is null || answer.AttemptId != attemptId)
                return _responseHandler.NotFound<AttemptAnswerResponseDto>("Answer not found.");

            if (answer.Attempt.ExamId != examId)
                return _responseHandler.NotFound<AttemptAnswerResponseDto>("Answer not found.");

            return _responseHandler.Success(MapToDto(answer));
        }

        // ─────────────────────────────────────────────────────────
        // GET /api/exams/{examId}/attempts/{attemptId}/answers/text
        // Returns only text answers that need manual grading
        // ─────────────────────────────────────────────────────────
        public async Task<Response<IReadOnlyList<AttemptAnswerResponseDto>>> GetTextAnswersAsync(
            int examId, int attemptId, CancellationToken ct)
        {
            var attempt = await _unitOfWork.ExamAttempts.GetByIdAsync(attemptId, ct);
            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<IReadOnlyList<AttemptAnswerResponseDto>>(
                    "Attempt not found.");

            var answers = await _unitOfWork.ExamAttemptAnswers
                .GetTextAnswersByAttemptAsync(attemptId, ct);

            var result = answers.Select(MapToDto).ToList();
            return _responseHandler.Success<IReadOnlyList<AttemptAnswerResponseDto>>(result);
        }

        // ─────────────────────────────────────────────────────────
        // PATCH /api/exams/{examId}/attempts/{attemptId}/answers/{questionId}/grade
        // Instructor grades a single text answer
        // ─────────────────────────────────────────────────────────
        public async Task<Response<AttemptAnswerResponseDto>> GradeAnswerAsync(
            int examId, int attemptId, int questionId,
            UpdateAnswerScoreDto dto, CancellationToken ct)
        {
            // 1. Validate attempt belongs to exam
            var attempt = await _unitOfWork.ExamAttempts.GetByIdAsync(attemptId, ct);
            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<AttemptAnswerResponseDto>("Attempt not found.");

            // 2. Must be submitted before grading
            if (attempt.Status == ExamAttemptStatus.InProgress)
                return _responseHandler.BadRequest<AttemptAnswerResponseDto>(
                    "Cannot grade an in-progress attempt.");

            // 3. Find the answer
            var answer = await _unitOfWork.ExamAttemptAnswers
                .GetByAttemptAndQuestionAsync(attemptId, questionId, ct);

            if (answer is null)
                return _responseHandler.NotFound<AttemptAnswerResponseDto>("Answer not found.");

            // 4. Validate score does not exceed question points
            if (dto.Score > answer.Question.Points)
                return _responseHandler.BadRequest<AttemptAnswerResponseDto>(
                    $"Score cannot exceed question points ({answer.Question.Points}).");

            // 5. Apply grade
            answer.Score = dto.Score;
            answer.IsCorrect = dto.IsCorrect;

            // 6. Recalculate total attempt score
            var allAnswers = await _unitOfWork.ExamAttemptAnswers
                .GetByAttemptIdAsync(attemptId, ct);

            attempt.Score = allAnswers.Sum(a =>
                a.Id == answer.Id ? dto.Score : (a.Score ?? 0));

            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Success(MapToDto(answer));
        }

        // ─────────────────────────────────────────────────────────
        // PATCH /api/exams/{examId}/attempts/{attemptId}/answers/grade-all
        // Instructor grades all text answers at once
        // ─────────────────────────────────────────────────────────
        public async Task<Response<string>> BulkGradeAsync(
            int examId, int attemptId,
            BulkUpdateAnswerScoreDto dto, CancellationToken ct)
        {
            // 1. Validate attempt
            var attempt = await _unitOfWork.ExamAttempts.GetByIdAsync(attemptId, ct);
            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<string>("Attempt not found.");

            if (attempt.Status == ExamAttemptStatus.InProgress)
                return _responseHandler.BadRequest<string>(
                    "Cannot grade an in-progress attempt.");

            // 2. Load all answers for this attempt
            var allAnswers = await _unitOfWork.ExamAttemptAnswers
                .GetByAttemptIdAsync(attemptId, ct);

            // 3. Apply scores
            foreach (var gradeDto in dto.Answers)
            {
                var answer = allAnswers.FirstOrDefault(a => a.QuestionId == gradeDto.QuestionId);
                if (answer is null) continue;

                // Validate score does not exceed question points
                if (gradeDto.Score > answer.Question.Points)
                    return _responseHandler.BadRequest<string>(
                        $"Score for question {gradeDto.QuestionId} exceeds max points " +
                        $"({answer.Question.Points}).");

                answer.Score = gradeDto.Score;
                answer.IsCorrect = gradeDto.IsCorrect;
            }

            // 4. Recalculate total attempt score and pass/fail
            attempt.Score = allAnswers.Sum(a => a.Score ?? 0);
            attempt.IsPassed = attempt.Score >= attempt.Exam.PassingScore;
            attempt.Status = ExamAttemptStatus.UnderReview;

            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Success(
                $"Graded {dto.Answers.Count} answers. Total score: {attempt.Score}.");
        }

        // ─────────────────────────────────────────────────────────
        // Private mapper
        // ─────────────────────────────────────────────────────────
        private static AttemptAnswerResponseDto MapToDto(ExamAttemptAnswer a) => new AttemptAnswerResponseDto()
        {
            
            AttemptId = a.AttemptId,
            QuestionId = a.QuestionId,
            QuestionText = a.Question?.Text ?? string.Empty,
            SelectedOptionId = a.SelectedOptionId,
            SelectedOptionText = a.SelectedOption?.Text,
            TextAnswer = a.TextAnswer,
            Score = a.Score,
            IsCorrect = a.IsCorrect,
        };
    }
}
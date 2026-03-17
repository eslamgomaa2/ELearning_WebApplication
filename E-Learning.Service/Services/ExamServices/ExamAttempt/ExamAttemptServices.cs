using E_Learning.Core.Base;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Enums;
using E_Learning.Core.Repository;

namespace E_Learning.Service.Services.ExamServices.Attempts
{
    public class ExamAttemptServices : IExamAttemptServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;

        public ExamAttemptServices(IUnitOfWork unitOfWork, ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }
       public async Task<Response<StartAttemptResponseDto>> StartAsync(
    int examId, Guid studentId, CancellationToken ct)
{
    
    var exam = await _unitOfWork.Exams.GetByIdWithQuestionsAsync(examId, ct);
    if (exam is null || !exam.IsActive)
        return _responseHandler.NotFound<StartAttemptResponseDto>(
            "Exam not found or not active.");

    if (exam.EndDateTime.HasValue && exam.EndDateTime.Value < DateTime.UtcNow)
        return _responseHandler.BadRequest<StartAttemptResponseDto>(
            "Exam has already ended.");

    var attemptCount = await _unitOfWork.ExamAttempts
        .CountAttemptsAsync(studentId, examId, ct);
    if (attemptCount >= exam.MaxAttempts)
        return _responseHandler.BadRequest<StartAttemptResponseDto>(
            $"You have reached the maximum number of attempts ({exam.MaxAttempts}).");

    var activeAttempt = await _unitOfWork.ExamAttempts
        .GetActiveAttemptAsync(studentId, examId, ct);
    if (activeAttempt is not null)
        return _responseHandler.BadRequest<StartAttemptResponseDto>(
            "You already have an active attempt. Please submit it first.");

    var attempt = new ExamAttempt
    {
        StudentId = studentId,
        ExamId    = examId,
        StartedAt = DateTime.UtcNow,
        Status    = ExamAttemptStatus.InProgress,
    };

    await _unitOfWork.ExamAttempts.AddAsync(attempt, ct);
    await _unitOfWork.SaveChangesAsync(ct);

    
    var questions = exam.Questions.ToList();

    if (exam.AIShuffleEnabled)
    {
     
        var seed = HashCode.Combine(studentId, examId, attempt.Id);
        var rng  = new Random(seed);
        questions = questions.OrderBy(_ => rng.Next()).ToList();
    }
    else
    {
        questions = questions.OrderBy(q => q.OrderIndex).ToList();
    }
    var questionDtos = questions.Select(q => new QuestionForStudentDto
    {
        Id           = q.Id,
        QuestionText = q.Text,
        QuestionType = q.Type,
        Points       = q.Points,
        Options      = q.Options
            .OrderBy(o => exam.AIShuffleEnabled
                ? Guid.NewGuid()
                : (object)o.OrderIndex)
            .Select(o => new OptionForStudentDto
            {
                Id         = o.Id,
                OptionText = o.Text,
                
            }).ToList()
    }).ToList();

    return _responseHandler.Created(new StartAttemptResponseDto
    {
        AttemptId      = attempt.Id,
        StartedAt      = attempt.StartedAt,
        ExamEndsAt     = attempt.StartedAt.AddSeconds(exam.DurationSeconds),
        TotalQuestions = questions.Count,
        Questions      = questionDtos,   
    });
}


        public async Task<Response<AttemptResponseDto>> SubmitAsync( int examId, int attemptId, Guid studentId, SubmitAttemptDto dto, CancellationToken ct)
        {
            // 1. Load attempt with exam + questions + options
            var attempt = await _unitOfWork.ExamAttempts.GetByIdWithExamAsync(attemptId, ct);

            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<AttemptResponseDto>("Attempt not found.");

            // 2. Must belong to this student
            if (attempt.StudentId != studentId)
                return _responseHandler.Forbidden<AttemptResponseDto>("Access denied.");

            // 3. Must still be in progress
            if (attempt.Status != ExamAttemptStatus.InProgress)
                return _responseHandler.BadRequest<AttemptResponseDto>("Attempt already submitted.");

            // 4. Auto-expire check — if time ran out, mark as expired
            var deadline = attempt.StartedAt.AddSeconds(attempt.Exam.DurationSeconds);
            if (DateTime.UtcNow > deadline)
                attempt.Status = ExamAttemptStatus.Expired;
            else
                attempt.Status = ExamAttemptStatus.Submitted;

            // 5. Grade each answer
            decimal totalScore = 0;

            foreach (var answerDto in dto.Answers)
            {
                var question = attempt.Exam.Questions
                    .FirstOrDefault(q => q.Id == answerDto.QuestionId);

                if (question is null) continue;

                bool? isCorrect = null;
                decimal? score = null;

                if (question.Type == "MCQ" || question.Type == "TrueFalse")
                {
                    var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                    isCorrect = correctOption is not null &&
                                correctOption.Id == answerDto.SelectedOptionId;
                    score = isCorrect == true ? question.Points : 0;
                }
                // Text questions — scored manually by instructor later
                // isCorrect and score stay null

                if (score.HasValue)
                    totalScore += score.Value;

                attempt.Answers.Add(new ExamAttemptAnswer
                {
                    QuestionId = answerDto.QuestionId,
                    SelectedOptionId = answerDto.SelectedOptionId,
                    TextAnswer = answerDto.TextAnswer,
                    IsCorrect = isCorrect,
                    Score = score,
                });
            }

            // 6. Set final score and pass/fail for auto-gradable questions
            attempt.Score = totalScore;
            attempt.SubmittedAt = DateTime.UtcNow;

            // IsPassed only auto-set if there are no text questions needing manual review
            bool hasTextQuestions = attempt.Exam.Questions.Any(q => q.Type == "Text");
            if (!hasTextQuestions)
                attempt.IsPassed = totalScore >= attempt.Exam.PassingScore;

            await _unitOfWork.SaveChangesAsync(ct);

            // Reload with full details for response
            var full = await _unitOfWork.ExamAttempts.GetByIdWithDetailsAsync(attempt.Id, ct);
            return _responseHandler.Success(MapToDto(full!));
        }

        // ─────────────────────────────────────────────────────
        // GET /api/exams/{examId}/attempts/{attemptId}
        // ─────────────────────────────────────────────────────
        public async Task<Response<AttemptResponseDto>> GetByIdAsync(
            int examId, int attemptId, CancellationToken ct)
        {
            var attempt = await _unitOfWork.ExamAttempts.GetByIdWithDetailsAsync(attemptId, ct);

            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<AttemptResponseDto>("Attempt not found.");

            return _responseHandler.Success(MapToDto(attempt));
        }

        // ─────────────────────────────────────────────────────
        // GET /api/exams/{examId}/attempts/my
        // ─────────────────────────────────────────────────────
        public async Task<Response<IReadOnlyList<AttemptResponseDto>>> GetMyAttemptsAsync(
            int examId, Guid studentId, CancellationToken ct)
        {
            var examExists = await _unitOfWork.Exams.AnyAsync(e => e.Id == examId, ct);
            if (!examExists)
                return _responseHandler.NotFound<IReadOnlyList<AttemptResponseDto>>("Exam not found.");

            var attempts = await _unitOfWork.ExamAttempts
                .GetByStudentAndExamAsync(studentId, examId, ct);

            var result = attempts.Select(MapToDto).ToList();
            return _responseHandler.Success<IReadOnlyList<AttemptResponseDto>>(result);
        }

        // ─────────────────────────────────────────────────────
        // GET /api/exams/{examId}/attempts  (instructor)
        // ─────────────────────────────────────────────────────
        public async Task<Response<IReadOnlyList<AttemptResponseDto>>> GetAllByExamAsync(
            int examId, CancellationToken ct)
        {
            var examExists = await _unitOfWork.Exams.AnyAsync(e => e.Id == examId, ct);
            if (!examExists)
                return _responseHandler.NotFound<IReadOnlyList<AttemptResponseDto>>("Exam not found.");

            var attempts = await _unitOfWork.ExamAttempts.GetByExamIdAsync(examId, ct);

            var result = attempts.Select(MapToDto).ToList();
            return _responseHandler.Success<IReadOnlyList<AttemptResponseDto>>(result);
        }

        // ─────────────────────────────────────────────────────
        // PATCH /api/exams/{examId}/attempts/{attemptId}/review
        // ─────────────────────────────────────────────────────
        public async Task<Response<AttemptResponseDto>> ReviewAsync(
            int examId, int attemptId, Guid reviewerId, ReviewAttemptDto dto, CancellationToken ct)
        {
            var attempt = await _unitOfWork.ExamAttempts.GetByIdWithDetailsAsync(attemptId, ct);

            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<AttemptResponseDto>("Attempt not found.");

            if (attempt.Status != ExamAttemptStatus.Submitted)
                return _responseHandler.BadRequest<AttemptResponseDto>(
                    "Only submitted attempts can be reviewed.");

            attempt.ReviewDecision = dto.ReviewDecision;
            attempt.TeacherComment = dto.TeacherComment;
            attempt.ReviewedBy = reviewerId;
            attempt.ReviewedAt = DateTime.UtcNow;
            attempt.Status = ExamAttemptStatus.UnderReview;

            // Recalculate final score including any manual text question scores
            var totalScore = attempt.Answers.Sum(a => a.Score ?? 0);
            attempt.Score = totalScore;
            attempt.IsPassed = totalScore >= attempt.Exam.PassingScore;

            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Success(MapToDto(attempt));
        }

        // ─────────────────────────────────────────────────────
        // PATCH /api/exams/{examId}/attempts/{attemptId}/publish
        // ─────────────────────────────────────────────────────
        public async Task<Response<string>> PublishAsync(
            int examId, int attemptId, CancellationToken ct)
        {
            var attempt = await _unitOfWork.ExamAttempts
                .GetByIdAsync(attemptId, ct);

            if (attempt is null || attempt.ExamId != examId)
                return _responseHandler.NotFound<string>("Attempt not found.");

            if (attempt.IsPublished)
                return _responseHandler.BadRequest<string>("Result already published.");

            attempt.IsPublished = true;
            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Success("Result published successfully.");
        }

        // ─────────────────────────────────────────────────────
        // Private mapper
        // ─────────────────────────────────────────────────────
        private static AttemptResponseDto MapToDto(ExamAttempt a) => new()
        {
            Id = a.Id,
            StudentId = a.StudentId,
            StudentName = a.Student?.UserName ?? string.Empty,
            ExamId = a.ExamId,
            StartedAt = a.StartedAt,
            SubmittedAt = a.SubmittedAt,
            Score = a.Score,
            IsPassed = a.IsPassed,
            Status = a.Status.ToString(),
            IsPublished = a.IsPublished,
            TeacherComment = a.TeacherComment,
            ReviewDecision = a.ReviewDecision,
            Answers = a.Answers.Select(ans => new AttemptAnswerResponseDto
            {
                QuestionId = ans.QuestionId,
                QuestionText = ans.Question?.Text ?? string.Empty,
                SelectedOptionId = ans.SelectedOptionId,
                SelectedOptionText = ans.SelectedOption?.Text,
                TextAnswer = ans.TextAnswer,
                IsCorrect = ans.IsCorrect,
                Score = ans.Score,
            }).ToList()
        };
    }
}

using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Base;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs;
using E_Learning.core.Interfaces.Repositories.Assessments.Quizzes;
using Microsoft.EntityFrameworkCore;


namespace E_Learning.Service.Services.QuizServices
{
    public class QuizService : IQuizService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;

        public QuizService(IUnitOfWork unitOfWork, ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        // ─── Create ───────────────────────────────────────────────────────────

        public async Task<Response<QuizDetailResponseDto>> CreateAsync(CreateQuizDto dto, CancellationToken ct = default)
        {
            // Validate course exists
            var courseExists = await _unitOfWork.Courses.AnyAsync(c => c.Id == dto.CourseId, ct);
            if (!courseExists)
                return _responseHandler.NotFound<QuizDetailResponseDto>("Course not found.");
            // Validate lessonId if provided ← هنا


            // Validate no duplicate title in same course
            var titleExists = await _unitOfWork.Quizzes.ExistsByTitleAsync(dto.Title, dto.CourseId, ct);
            if (titleExists)
                return _responseHandler.BadRequest<QuizDetailResponseDto>("A quiz with this title already exists in the course.");

            // Validate questions count matches
            if (dto.Questions.Any() && dto.Questions.Count != dto.Questions.Count)
                return _responseHandler.BadRequest<QuizDetailResponseDto>("Questions count mismatch.");

            // Map to entity
            var quiz = new Quiz
            {
                Title = dto.Title,
                CourseId = dto.CourseId,
                LessonId = dto.LessonId,
                Topic = dto.Topic,
                Type = dto.Type,
                TimeLimitSeconds = dto.TimeLimitSeconds,
                TimePerQuestionSeconds = dto.TimePerQuestionSeconds,
                PassingScore = dto.PassingScore,
                MaxAttempts = dto.MaxAttempts,
                ShuffleQuestions = dto.ShuffleQuestions,
                ShowResultsImmediately = dto.ShowResultsImmediately,
                IsActive = true,
                Questions = dto.Questions.Select((q, qi) => new QuizQuestion
                {
                    Text = q.Text,
                    Type = q.Type,
                    Points = q.Points,
                    OrderIndex = q.OrderIndex > 0 ? q.OrderIndex : qi + 1,
                    Options = q.Options.Select((o, oi) => new QuizOption
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect,
                        OrderIndex = o.OrderIndex > 0 ? o.OrderIndex : oi + 1
                    }).ToList()
                }).ToList()
            };

            await _unitOfWork.Quizzes.AddAsync(quiz, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            // Reload with full includes
            var created = await _unitOfWork.Quizzes.GetWithQuestionsAndOptionsAsync(quiz.Id, ct);
            var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId, ct);

            return _responseHandler.Created(MapToDetailDto(created!, course?.Title ?? ""));
        }

        // ─── Get By Id ────────────────────────────────────────────────────────

        public async Task<Response<QuizDetailResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var quiz = await _unitOfWork.Quizzes.GetWithQuestionsAndOptionsAsync(id, ct);
            if (quiz is null)
                return _responseHandler.NotFound<QuizDetailResponseDto>("Quiz not found.");

            var course = await _unitOfWork.Courses.GetByIdAsync(quiz.CourseId, ct);
            return _responseHandler.Success(MapToDetailDto(quiz, course?.Title ?? ""));
        }

        // ─── Get All ──────────────────────────────────────────────────────────

        public async Task<Response<IReadOnlyList<QuizResponseDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var quizzes = await _unitOfWork.Quizzes.GetAllAsync(
                include: q => q.Include(x => x.Questions).Include(x => x.Course),
                ct: ct);

            var result = quizzes.Select(q => MapToDto(q, q.Course?.Title ?? "")).ToList();
            return _responseHandler.Success<IReadOnlyList<QuizResponseDto>>(result);
        }

        // ─── Get By Course ────────────────────────────────────────────────────

        public async Task<Response<IReadOnlyList<QuizResponseDto>>> GetByCourseIdAsync(int courseId, CancellationToken ct = default)
        {
            var courseExists = await _unitOfWork.Courses.AnyAsync(c => c.Id == courseId, ct);
            if (!courseExists)
                return _responseHandler.NotFound<IReadOnlyList<QuizResponseDto>>("Course not found.");

            var quizzes = await _unitOfWork.Quizzes.GetByCourseIdAsync(courseId, ct);
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId, ct);

            var result = quizzes.Select(q => MapToDto(q, course?.Title ?? "")).ToList();
            return _responseHandler.Success<IReadOnlyList<QuizResponseDto>>(result);
        }

        // ─── Update ───────────────────────────────────────────────────────────

        public async Task<Response<QuizResponseDto>> UpdateAsync(int id, UpdateQuizDto dto, CancellationToken ct = default)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id, ct);
            if (quiz is null)
                return _responseHandler.NotFound<QuizResponseDto>("Quiz not found.");

            quiz.Title = dto.Title;
            quiz.Topic = dto.Topic;
            quiz.Type = dto.Type;
            quiz.TimeLimitSeconds = dto.TimeLimitSeconds;
            quiz.TimePerQuestionSeconds = dto.TimePerQuestionSeconds;
            quiz.PassingScore = dto.PassingScore;
            quiz.MaxAttempts = dto.MaxAttempts;
            quiz.ShuffleQuestions = dto.ShuffleQuestions;
            quiz.ShowResultsImmediately = dto.ShowResultsImmediately;
            quiz.IsActive = dto.IsActive;

            _unitOfWork.Quizzes.Update(quiz);
            await _unitOfWork.SaveChangesAsync(ct);

            var course = await _unitOfWork.Courses.GetByIdAsync(quiz.CourseId, ct);
            return _responseHandler.Success(MapToDto(quiz, course?.Title ?? ""));
        }

        // ─── Delete ───────────────────────────────────────────────────────────

        public async Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id, ct);
            if (quiz is null)
                return _responseHandler.NotFound<string>("Quiz not found.");

            _unitOfWork.Quizzes.Remove(quiz);
            await _unitOfWork.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>("Quiz deleted successfully.");
        }

        // ─── Mapping Helpers ──────────────────────────────────────────────────

        private static QuizResponseDto MapToDto(Quiz quiz, string courseName) => new()
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Topic = quiz.Topic,
            Type = quiz.Type,
            CourseId = quiz.CourseId,
            CourseName = courseName,
            LessonId = quiz.LessonId,
            TimeLimitSeconds = quiz.TimeLimitSeconds,
            TimePerQuestionSeconds = quiz.TimePerQuestionSeconds,
            PassingScore = quiz.PassingScore,
            MaxAttempts = quiz.MaxAttempts,
            ShuffleQuestions = quiz.ShuffleQuestions,
            ShowResultsImmediately = quiz.ShowResultsImmediately,
            IsActive = quiz.IsActive,
            QuestionsCount = quiz.Questions?.Count ?? 0,
            CreatedAt = quiz.CreatedAt
        };

        private static QuizDetailResponseDto MapToDetailDto(Quiz quiz, string courseName) => new()
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Topic = quiz.Topic,
            Type = quiz.Type,
            CourseId = quiz.CourseId,
            CourseName = courseName,
            LessonId = quiz.LessonId,
            TimeLimitSeconds = quiz.TimeLimitSeconds,
            TimePerQuestionSeconds = quiz.TimePerQuestionSeconds,
            PassingScore = quiz.PassingScore,
            MaxAttempts = quiz.MaxAttempts,
            ShuffleQuestions = quiz.ShuffleQuestions,
            ShowResultsImmediately = quiz.ShowResultsImmediately,
            IsActive = quiz.IsActive,
            QuestionsCount = quiz.Questions?.Count ?? 0,
            CreatedAt = quiz.CreatedAt,
            Questions = quiz.Questions?.Select(q => new QuizQuestionResponseDto
            {
                Id = q.Id,
                Text = q.Text,
                Type = q.Type,
                Points = q.Points,
                OrderIndex = q.OrderIndex,
                Options = q.Options?.Select(o => new QuizOptionResponseDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    IsCorrect = o.IsCorrect,
                    OrderIndex = o.OrderIndex
                }).ToList() ?? new()
            }).ToList() ?? new()
        };
    }
}
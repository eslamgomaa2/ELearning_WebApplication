using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Repository;
using E_Learning.Infrastructure;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace E_Learning.Infrastructure.Repositories
{
    public class ExamAttemptAnswerRepository
        : GenericRepository<ExamAttemptAnswer,int>, IExamAttemptAnswerRepository
    {
        private readonly ELearningDbContext _context;

        public ExamAttemptAnswerRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ExamAttemptAnswer>> GetByAttemptIdAsync(
            int attemptId,
            CancellationToken ct = default)
        {
            return await _context.ExamAttemptAnswers
                .Where(a => a.AttemptId == attemptId)
                .Include(a => a.Question)
                    .ThenInclude(q => q.Options)
                .Include(a => a.SelectedOption)
                .OrderBy(a => a.Question.OrderIndex)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<ExamAttemptAnswer?> GetByIdWithDetailsAsync(
            int answerId,
            CancellationToken ct = default)
        {
            return await _context.ExamAttemptAnswers
                .Include(a => a.Question)
                    .ThenInclude(q => q.Options)
                .Include(a => a.SelectedOption)
                .Include(a => a.Attempt)
                .FirstOrDefaultAsync(a => a.Id == answerId, ct);
        }

        public async Task<ExamAttemptAnswer?> GetByAttemptAndQuestionAsync(
            int attemptId,
            int questionId,
            CancellationToken ct = default)
        {
            return await _context.ExamAttemptAnswers
                .Include(a => a.Question)
                .Include(a => a.SelectedOption)
                .FirstOrDefaultAsync(a =>
                    a.AttemptId == attemptId &&
                    a.QuestionId == questionId, ct);
        }

        public async Task<IReadOnlyList<ExamAttemptAnswer>> GetTextAnswersByAttemptAsync(
            int attemptId,
            CancellationToken ct = default)
        {
            return await _context.ExamAttemptAnswers
                .Where(a => a.AttemptId == attemptId &&
                            a.Question.Type == "Text")
                .Include(a => a.Question)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(
            int attemptId,
            int answerId,
            CancellationToken ct = default)
        {
            return await _context.ExamAttemptAnswers
                .AnyAsync(a => a.Id == answerId &&
                               a.AttemptId == attemptId, ct);
        }
    }
}
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Enums;
using E_Learning.Core.Repository;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace E_Learning.Infrastructure.Repositories
{
    public class ExamAttemptRepository : GenericRepository<ExamAttempt,int>, IExamAttemptRepository
    {
        private readonly ELearningDbContext _context;

        public ExamAttemptRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<ExamAttempt?> GetByIdWithDetailsAsync(  int attemptId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .Include(a => a.Student)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.Question)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.SelectedOption)
                .FirstOrDefaultAsync(a => a.Id == attemptId, ct);
        }

        public async Task<IReadOnlyList<ExamAttempt>> GetByExamIdAsync( int examId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .Where(a => a.ExamId == examId)
                .Include(a => a.Student)
                .OrderByDescending(a => a.StartedAt)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<ExamAttempt>> GetByStudentAndExamAsync(Guid studentId, int examId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .Where(a => a.StudentId == studentId && a.ExamId == examId)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.Question)
                .OrderByDescending(a => a.StartedAt)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<ExamAttempt?> GetActiveAttemptAsync(  Guid studentId, int examId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .FirstOrDefaultAsync(a =>
                    a.StudentId == studentId &&
                    a.ExamId == examId &&
                    a.Status == ExamAttemptStatus.InProgress, ct);
        }

        public async Task<int> CountAttemptsAsync(Guid studentId, int examId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .CountAsync(a => a.StudentId == studentId && a.ExamId == examId, ct);
        }

        public async Task<ExamAttempt?> GetByIdWithExamAsync(  int attemptId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .Include(a => a.Exam)
                    .ThenInclude(e => e.Questions)
                        .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(a => a.Id == attemptId, ct);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using E_Learning.Repository.Data;
using E_Learning.Service.Contract;
using E_Learning.Service.DTOs.Schedule;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.Schedule
{
    public class ScheduleService : IScheduleService
    {
        private readonly ELearningDbContext _db;

        public ScheduleService(ELearningDbContext db)
        {
            _db = db;
        }

        public async Task<List<CalendarDayDto>> GetCalendarAsync(Guid studentId, int month, int year)
        {
            var courseIds = await GetEnrolledCourseIds(studentId);
            if (!courseIds.Any()) return new();

            var startOfMonth = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1);

            var exams = await _db.Exams
                .Where(e => courseIds.Contains(e.CourseId) && e.IsActive
                    && e.ScheduledAt >= startOfMonth && e.ScheduledAt < endOfMonth)
                .Select(e => new CalendarEventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Type = "Exam",
                    StartDate = e.ScheduledAt
                }).ToListAsync();

            var assignments = await _db.Assignments
                .Where(a => courseIds.Contains(a.CourseId) && a.IsActive
                    && a.DueDate >= startOfMonth && a.DueDate < endOfMonth)
                .Select(a => new CalendarEventDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Type = "Assignment",
                    StartDate = a.DueDate
                }).ToListAsync();

            var quizzes = await _db.Quizzes
                .Where(q => courseIds.Contains(q.CourseId) && q.IsActive
                    && q.ScheduledAt != null
                    && q.ScheduledAt >= startOfMonth && q.ScheduledAt < endOfMonth)
                .Select(q => new CalendarEventDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Type = "Quiz",
                    StartDate = q.ScheduledAt!.Value
                }).ToListAsync();

            var sessions = await _db.LiveSessions
                .Where(s => courseIds.Contains(s.CourseId)
                    && s.ScheduledAt >= startOfMonth && s.ScheduledAt < endOfMonth)
                .Select(s => new CalendarEventDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Type = "LiveSession",
                    StartDate = s.ScheduledAt
                }).ToListAsync();

            var allEvents = exams.Concat(assignments).Concat(quizzes).Concat(sessions);

            return allEvents
                .GroupBy(e => DateOnly.FromDateTime(e.StartDate))
                .Select(g => new CalendarDayDto
                {
                    Date = g.Key,
                    Events = g.OrderBy(e => e.StartDate).ToList()
                })
                .OrderBy(d => d.Date)
                .ToList();
        }

        public async Task<List<ScheduleEventDto>> GetUpcomingEventsAsync(Guid studentId, ScheduleQueryDto query)
        {
            var courseIds = await GetEnrolledCourseIds(studentId);
            if (!courseIds.Any()) return new();

            var now = DateTime.UtcNow;
            var events = new List<ScheduleEventDto>();

            var filteredCourseIds = query.CourseId.HasValue
                ? courseIds.Where(id => id == query.CourseId.Value).ToList()
                : courseIds;

            if (string.IsNullOrEmpty(query.Type) || query.Type == "Exam")
            {
                var exams = await _db.Exams
                    .Include(e => e.Course)
                    .Where(e => filteredCourseIds.Contains(e.CourseId) && e.IsActive && e.ScheduledAt >= now)
                    .Select(e => new ScheduleEventDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Instructions,
                        Type = "Exam",
                        CourseName = e.Course.Title,
                        CourseId = e.CourseId,
                        StartDate = e.ScheduledAt,
                        EndDate = e.EndDateTime,
                        DurationMinutes = e.DurationSeconds / 60,
                        Priority = "High"
                    }).ToListAsync();
                events.AddRange(exams);
            }

            if (string.IsNullOrEmpty(query.Type) || query.Type == "Assignment")
            {
                var assignments = await _db.Assignments
                    .Include(a => a.Course)
                    .Where(a => filteredCourseIds.Contains(a.CourseId) && a.IsActive && a.DueDate >= now)
                    .Select(a => new ScheduleEventDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Description = a.Description,
                        Type = "Assignment",
                        CourseName = a.Course.Title,
                        CourseId = a.CourseId,
                        StartDate = a.DueDate,
                        Priority = a.DueDate <= now.AddDays(2) ? "High" : "Medium"
                    }).ToListAsync();
                events.AddRange(assignments);
            }

            if (string.IsNullOrEmpty(query.Type) || query.Type == "Quiz")
            {
                var quizzes = await _db.Quizzes
                    .Include(q => q.Course)
                    .Where(q => filteredCourseIds.Contains(q.CourseId) && q.IsActive
                        && q.ScheduledAt != null && q.ScheduledAt >= now)
                    .Select(q => new ScheduleEventDto
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Description = q.Topic,
                        Type = "Quiz",
                        CourseName = q.Course.Title,
                        CourseId = q.CourseId,
                        StartDate = q.ScheduledAt!.Value,
                        DurationMinutes = q.TimeLimitSeconds.HasValue ? q.TimeLimitSeconds.Value / 60 : null,
                        Priority = "Medium"
                    }).ToListAsync();
                events.AddRange(quizzes);
            }

            if (string.IsNullOrEmpty(query.Type) || query.Type == "LiveSession")
            {
                var sessions = await _db.LiveSessions
                    .Include(s => s.Course)
                    .Where(s => filteredCourseIds.Contains(s.CourseId) && s.ScheduledAt >= now)
                    .Select(s => new ScheduleEventDto
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        Type = "LiveSession",
                        CourseName = s.Course.Title,
                        CourseId = s.CourseId,
                        StartDate = s.ScheduledAt,
                        EndDate = s.ScheduledAt.AddMinutes(s.DurationMinutes),
                        DurationMinutes = s.DurationMinutes,
                        Priority = "Medium",
                        Status = s.Status.ToString()
                    }).ToListAsync();
                events.AddRange(sessions);
            }

            return events
                .OrderBy(e => e.StartDate)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();
        }

        public async Task<DeadlineSummaryDto> GetDeadlineSummaryAsync(Guid studentId)
        {
            var courseIds = await GetEnrolledCourseIds(studentId);
            if (!courseIds.Any()) return new();

            var now = DateTime.UtcNow;
            var nextWeek = now.AddDays(7);

            var upcomingExams = await _db.Exams
                .CountAsync(e => courseIds.Contains(e.CourseId) && e.IsActive
                    && e.ScheduledAt >= now && e.ScheduledAt <= nextWeek);

            var upcomingAssignments = await _db.Assignments
                .CountAsync(a => courseIds.Contains(a.CourseId) && a.IsActive
                    && a.DueDate >= now && a.DueDate <= nextWeek);

            var upcomingQuizzes = await _db.Quizzes
                .CountAsync(q => courseIds.Contains(q.CourseId) && q.IsActive
                    && q.ScheduledAt != null && q.ScheduledAt >= now && q.ScheduledAt <= nextWeek);

            var upcomingSessions = await _db.LiveSessions
                .CountAsync(s => courseIds.Contains(s.CourseId)
                    && s.ScheduledAt >= now && s.ScheduledAt <= nextWeek);

            var total = upcomingExams + upcomingAssignments + upcomingQuizzes + upcomingSessions;

            var completedExams = await _db.ExamAttempts
                .CountAsync(a => a.StudentId == studentId && a.SubmittedAt != null
                    && a.Exam.ScheduledAt >= now.AddDays(-7) && a.Exam.ScheduledAt <= nextWeek);

            var completedAssignments = await _db.AssignmentSubmissions
                .CountAsync(s => s.StudentId == studentId && s.SubmittedAt != null
                    && s.Assignment.DueDate >= now.AddDays(-7) && s.Assignment.DueDate <= nextWeek);

            var completedQuizzes = await _db.QuizAttempts
                .CountAsync(a => a.StudentId == studentId && a.SubmittedAt != null
                    && a.Quiz.ScheduledAt != null
                    && a.Quiz.ScheduledAt >= now.AddDays(-7) && a.Quiz.ScheduledAt <= nextWeek);

            var totalAll = total + completedExams + completedAssignments + completedQuizzes;
            var completionPct = totalAll > 0
                ? (double)(completedExams + completedAssignments + completedQuizzes) / totalAll * 100
                : 0;

            return new DeadlineSummaryDto
            {
                UpcomingExams = upcomingExams,
                UpcomingAssignments = upcomingAssignments,
                UpcomingQuizzes = upcomingQuizzes,
                UpcomingLiveSessions = upcomingSessions,
                TotalUpcoming = total,
                CompletionPercentage = Math.Round(completionPct, 1)
            };
        }

        public async Task<ScheduleEventDto> GetEventDetailsAsync(Guid studentId, string type, int id)
        {
            var courseIds = await GetEnrolledCourseIds(studentId);

            return type.ToLower() switch
            {
                "exam" => await GetExamDetail(courseIds, id),
                "assignment" => await GetAssignmentDetail(courseIds, id),
                "quiz" => await GetQuizDetail(courseIds, id),
                "livesession" => await GetSessionDetail(courseIds, id),
                _ => throw new Exception("Invalid event type. Use: Exam, Assignment, Quiz, LiveSession")
            };
        }

        private async Task<List<int>> GetEnrolledCourseIds(Guid studentId)
        {
            return await _db.Enrollments
                .Where(e => e.StudentId == studentId && !e.IsDeleted)
                .Select(e => e.CourseId)
                .Distinct()
                .ToListAsync();
        }

        private async Task<ScheduleEventDto> GetExamDetail(List<int> courseIds, int id)
        {
            var exam = await _db.Exams.Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id && courseIds.Contains(e.CourseId));
            if (exam == null) throw new Exception("Exam not found.");

            return new ScheduleEventDto
            {
                Id = exam.Id,
                Title = exam.Title,
                Description = exam.Instructions,
                Type = "Exam",
                CourseName = exam.Course.Title,
                CourseId = exam.CourseId,
                StartDate = exam.ScheduledAt,
                EndDate = exam.EndDateTime,
                DurationMinutes = exam.DurationSeconds / 60,
                Priority = "High"
            };
        }

        private async Task<ScheduleEventDto> GetAssignmentDetail(List<int> courseIds, int id)
        {
            var a = await _db.Assignments.Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == id && courseIds.Contains(a.CourseId));
            if (a == null) throw new Exception("Assignment not found.");

            return new ScheduleEventDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Type = "Assignment",
                CourseName = a.Course.Title,
                CourseId = a.CourseId,
                StartDate = a.DueDate,
                Priority = a.DueDate <= DateTime.UtcNow.AddDays(2) ? "High" : "Medium"
            };
        }

        private async Task<ScheduleEventDto> GetQuizDetail(List<int> courseIds, int id)
        {
            var q = await _db.Quizzes.Include(q => q.Course)
                .FirstOrDefaultAsync(q => q.Id == id && courseIds.Contains(q.CourseId));
            if (q == null) throw new Exception("Quiz not found.");

            return new ScheduleEventDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Topic,
                Type = "Quiz",
                CourseName = q.Course.Title,
                CourseId = q.CourseId,
                StartDate = q.ScheduledAt ?? q.CreatedAt,
                DurationMinutes = q.TimeLimitSeconds.HasValue ? q.TimeLimitSeconds.Value / 60 : null,
                Priority = "Medium"
            };
        }

        private async Task<ScheduleEventDto> GetSessionDetail(List<int> courseIds, int id)
        {
            var s = await _db.LiveSessions.Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id && courseIds.Contains(s.CourseId));
            if (s == null) throw new Exception("Live session not found.");

            return new ScheduleEventDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Type = "LiveSession",
                CourseName = s.Course.Title,
                CourseId = s.CourseId,
                StartDate = s.ScheduledAt,
                EndDate = s.ScheduledAt.AddMinutes(s.DurationMinutes),
                DurationMinutes = s.DurationMinutes,
                Priority = "Medium",
                Status = s.Status.ToString()
            };
        }
    }
}
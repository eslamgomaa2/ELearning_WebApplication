using E_learning.Core.Entities.Identity;
using E_learning.Repository.Interceptors;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Entities.Assessments.Quiz;
using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Core.Entities.Identity;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Entities.Reviews;
using E_Learning.Core.Entities.Schedule;
using E_Learning.Core.Entities.Support;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace E_Learning.Repository.Data
{
    public class ELearningDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        private readonly AuditInterceptor _auditInterceptor;

        public ELearningDbContext(DbContextOptions<ELearningDbContext> options, AuditInterceptor? auditInterceptor = null) : base(options)
        {
            _auditInterceptor = auditInterceptor!;
        }

        // ─── Identity ────────────────────────────
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }

        // ─── Profiles ────────────────────────────
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<InstructorProfile> InstructorProfiles { get; set; }
        public DbSet<AdminProfile> AdminProfiles { get; set; }

        // ─── Academic ────────────────────────────
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Level> Levels { get; set; }

        // ─── Courses ─────────────────────────────
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        // ─── Enrollment ──────────────────────────
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }

        // ─── Assessments - Quiz ───────────────────
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizOption> QuizOptions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizAttemptAnswer> QuizAttemptAnswers { get; set; }

        // ─── Assessments - Exam ───────────────────
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamOption> ExamOptions { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<ExamAttemptAnswer> ExamAttemptAnswers { get; set; }

        // ─── Assessments - Assignment ─────────────
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

        // ─── Live Sessions ────────────────────────
        public DbSet<LiveSession> LiveSessions { get; set; }
        public DbSet<LiveSessionAttendee> LiveSessionAttendees { get; set; }

        // ─── Reviews & Certificates ───────────────
        public DbSet<CourseReview> CourseReviews { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        // ─── Schedule ────────────────────────────
        public DbSet<ScheduleEvent> ScheduleEvents { get; set; }
        public DbSet<StudyReminder> StudyReminders { get; set; }

        // ─── Billing ─────────────────────────────
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<InstructorEarning> InstructorEarnings { get; set; }
        public DbSet<PayoutRequest> PayoutRequests { get; set; }
        public DbSet<PayoutApproval> PayoutApprovals { get; set; }

        // ─── Notifications ────────────────────────
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSetting> NotificationSettings { get; set; }

        // ─── Support ─────────────────────────────
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<SupportTicketReply> SupportTicketReplies { get; set; }
        public DbSet<CourseAnalyticsSnapshot> CourseAnalyticsSnapshots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ELearningDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(
                        entityType.ClrType, "e");

                    var property = Expression.Property(
                        parameter, nameof(ISoftDelete.IsDeleted));

                    var filter = Expression.Lambda(
                        Expression.Equal(
                            property,
                            Expression.Constant(false)),
                        parameter);

                    entityType.SetQueryFilter(filter);
                }
            }
        }



        // ─── OnConfiguring ───────────────────────
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_auditInterceptor != null)
                optionsBuilder.AddInterceptors(_auditInterceptor);
        }

    }
}
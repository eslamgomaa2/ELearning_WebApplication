using E_Learning.core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.core.Interfaces.Repositories.Assessments.Quizzes;
using E_Learning.core.Interfaces.Repositories.Authentications;
using E_Learning.core.Interfaces.Repositories.Courses;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Academic;
using E_Learning.Core.Interfaces.Repositories.AdminOperations;
using E_Learning.Core.Interfaces.Repositories.Assessments.Assignments;
using E_Learning.Core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories.Enrollments;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Core.Interfaces.Repositories.Notifications;
using E_Learning.Core.Interfaces.Repositories.Payments;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Core.Interfaces.Repositories.Reviews_Certificates;
using E_Learning.Core.Interfaces.Repositories.Schedule;

namespace E_Learning.Core.Repository
{
    public interface IUnitOfWork :IAsyncDisposable
    {
        IAppUserRepository AppUserRepository { get; }
        #region Authentications
        IUserSessionRepository UserSessions { get; }
        IOtpCodeRepository OtpCodes { get; }
        #endregion

        #region Profiles
        IStudentProfileRepository StudentProfiles { get; }
        IInstructorProfileRepository InstructorProfiles { get; }
        IAdminProfileRepository AdminProfiles { get; }
        #endregion

        #region Academic
        IStageRepository Stages { get; }
        ILevelRepository Levels { get; }
        #endregion
        #region Courses
        ICourseRepository Courses { get; }
        ISectionRepository Sections { get; }
        ILessonRepository Lessons { get; }
        #endregion

        #region Enrollments
        IEnrollmentRepository Enrollments { get; }
        ILessonProgressRepository LessonProgresses { get; }
        #endregion
        #region Assessments
        IQuizRepository Quizzes { get; }
        IQuizQuestionRepository QuizQuestions { get; }
        IQuizOptionRepository QuizOptions { get; }
        IQuizAttemptRepository QuizAttempts { get; }
        IQuizAttemptAnswerRepository QuizAttemptAnswers { get; }
        IExamRepository Exams { get; }
        IExamQuestionRepository ExamQuestions { get; }
        IExamOptionRepository ExamOptions { get; }
        IExamAttemptRepository ExamAttempts { get; }
        IExamAttemptAnswerRepository ExamAttemptAnswers { get; }
        IAssignmentRepository Assignments { get; }
        IAssignmentSubmissionRepository AssignmentSubmissions { get; }
        #endregion

        #region Live Sessions
        ILiveSessionRepository LiveSessions { get; }
        ILiveSessionAttendeeRepository LiveSessionAttendees { get; }
        #endregion

         #region Reviews & Certificates
        ICourseReviewRepository CourseReviews { get; }
        ICertificateRepository Certificates { get; }
        #endregion

        #region Schedule
        IScheduleEventRepository ScheduleEvents { get; }
        IStudyReminderRepository StudyReminders { get; }
        #endregion

        #region Payments
        IPaymentMethodRepository PaymentMethods { get; }
        IPaymentTransactionRepository PaymentTransactions { get; }
        IInstructorEarningRepository InstructorEarnings { get; }
        IPayoutRequestRepository PayoutRequests { get; }
        #endregion

        #region Notifications 
        INotificationRepository Notifications { get; }
        INotificationSettingsRepository NotificationSettings { get; }
        #endregion

        #region Admin Operations
        IPayoutApprovalRepository PayoutApprovals { get; }
        ISupportTicketRepository SupportTickets { get; }
        ISupportTicketReplyRepository SupportTicketReplies { get; }
        ICourseAnalyticsSnapshotRepository CourseAnalyticsSnapshots { get; }
        #endregion

        #region Transaction Control

        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
        #endregion
    }

}

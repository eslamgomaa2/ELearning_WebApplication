using E_Learning.core.Interfaces.Repositories.Assessments.Exams;
using E_Learning.Core.Interfaces.Repositories.Assessments.Quizzes;
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
using E_Learning.Core.Repository;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;
using E_Learning.Repository.Repositories.GenericesRepositories.Academic;
using E_Learning.Repository.Repositories.GenericesRepositories.AdminOperations;
using E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Assignments;
using E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Exams;
using E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Quizzes;
using E_Learning.Repository.Repositories.GenericesRepositories.Courses;
using E_Learning.Repository.Repositories.GenericesRepositories.Enrollments;
using E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions;
using E_Learning.Repository.Repositories.GenericesRepositories.Notifications;
using E_Learning.Repository.Repositories.GenericesRepositories.Payments;
using E_Learning.Repository.Repositories.GenericesRepositories.Profile;
using E_Learning.Repository.Repositories.GenericesRepositories.Reviews_Certificates;
using E_Learning.Repository.Repositories.GenericesRepositories.Schedule;



using Microsoft.EntityFrameworkCore.Storage;

namespace E_Learning.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ELearningDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ELearningDbContext context) => _context = context;

        private IAppUserRepository _userRepository;
        private IUserSessionRepository? _userSessions;
        private IOtpCodeRepository? _otpCodes;
        private IStudentProfileRepository? _studentProfiles;
        private Core.Interfaces.Repositories.Profile.IInstructorProfileRepository? _instructorProfiles;
        private IAdminProfileRepository? _adminProfiles;
        private IStageRepository? _stages;
        private ILevelRepository? _levels;
        private ICourseRepository? _courses;
        private ISectionRepository? _sections;
        private ILessonRepository? _lessons;
        private IEnrollmentRepository? _enrollments;
        private ILessonProgressRepository? _lessonProgresses;
        private IQuizRepository? _quizzes;
        private IQuizQuestionRepository? _quizQuestions;
        private IQuizOptionRepository? _quizOptions;
       
        private IExamRepository? _exams;
        private IExamQuestionRepository? _examQuestions;
        private IExamOptionRepository? _examOptions;
        private IExamAttemptRepository? _examAttempts;
        private IExamAttemptAnswerRepository? _examAttemptAnswers;
        private IAssignmentRepository? _assignments;
        private IAssignmentSubmissionRepository? _assignmentSubmissions;
        private ILiveSessionRepository? _liveSessions;
        private ILiveSessionAttendeeRepository? _liveSessionAttendees;
        private ICourseReviewRepository? _courseReviews;
        private ICertificateRepository? _certificates;
        private IScheduleEventRepository? _scheduleEvents;
        private IStudyReminderRepository? _studyReminders;
        private IPaymentMethodRepository? _paymentMethods;
        private IPaymentTransactionRepository? _paymentTransactions;
        private IInstructorEarningRepository? _instructorEarnings;
        private IPayoutRequestRepository? _payoutRequests;
        private INotificationRepository? _notifications;
        private INotificationSettingsRepository? _notificationSettings;
        private IPayoutApprovalRepository? _payoutApprovals;
        private ISupportTicketRepository? _supportTickets;
        private ISupportTicketReplyRepository? _supportTicketReplies;
        private ICourseAnalyticsSnapshotRepository? _courseAnalyticsSnapshots;


        public IUserSessionRepository UserSessions => _userSessions ??= new UserSessionRepository(_context);
        public IOtpCodeRepository OtpCodes => _otpCodes ??= new OtpCodeRepository(_context);
        public IStudentProfileRepository StudentProfiles => _studentProfiles ??= new StudentProfileRepository(_context);
        public Core.Interfaces.Repositories.Profile.IInstructorProfileRepository InstructorProfiles => _instructorProfiles ??= new GenericesRepositories.Profile.InstructorProfileRepository(_context);
        public IAdminProfileRepository AdminProfiles => _adminProfiles ??= new AdminProfileRepository(_context);
        public IStageRepository Stages => _stages ??= new StageRepository(_context);
        public ILevelRepository Levels => _levels ??= new LevelRepository(_context);
        public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
        public ISectionRepository Sections => _sections ??= new SectionRepository(_context);
        public ILessonRepository Lessons => _lessons ??= new LessonRepository(_context);
        public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(_context);
        public ILessonProgressRepository LessonProgresses => _lessonProgresses ??= new LessonProgressRepository(_context);
        public IQuizRepository Quizzes => _quizzes ??= new QuizRepository(_context);
        public IQuizQuestionRepository QuizQuestions => _quizQuestions ??= new QuizQuestionRepository(_context);
        public IQuizOptionRepository QuizOptions => _quizOptions ??= new QuizOptionRepository(_context);
       
        public IExamRepository Exams => _exams ??= new ExamRepository(_context);
        public IExamQuestionRepository ExamQuestions => _examQuestions ??= new ExamQuestionRepository(_context);
        public IExamOptionRepository ExamOptions => _examOptions ??= new ExamOptionRepository(_context);
        public IExamAttemptRepository ExamAttempts => _examAttempts ??= new ExamAttemptRepository(_context);
        public IExamAttemptAnswerRepository ExamAttemptAnswers => _examAttemptAnswers ??= new ExamAttemptAnswerRepository(_context);
        public IAssignmentRepository Assignments => _assignments ??= new AssignmentRepository(_context);
        public IAssignmentSubmissionRepository AssignmentSubmissions => _assignmentSubmissions ??= new AssignmentSubmissionRepository(_context);
        public ILiveSessionRepository LiveSessions => _liveSessions ??= new LiveSessionRepository(_context);
        public ILiveSessionAttendeeRepository LiveSessionAttendees => _liveSessionAttendees ??= new LiveSessionAttendeeRepository(_context);
        public ICourseReviewRepository CourseReviews => _courseReviews ??= new CourseReviewRepository(_context);
        public ICertificateRepository Certificates => _certificates ??= new CertificateRepository(_context);
        public IScheduleEventRepository ScheduleEvents => _scheduleEvents ??= new ScheduleEventRepository(_context);
        public IStudyReminderRepository StudyReminders => _studyReminders ??= new StudyReminderRepository(_context);
        public IPaymentMethodRepository PaymentMethods => _paymentMethods ??= new PaymentMethodRepository(_context);
        public IPaymentTransactionRepository PaymentTransactions => _paymentTransactions ??= new PaymentTransactionRepository(_context);
        public IInstructorEarningRepository InstructorEarnings => _instructorEarnings ??= new InstructorEarningRepository(_context);
        public IPayoutRequestRepository PayoutRequests => _payoutRequests ??= new PayoutRequestRepository(_context);
        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
        public INotificationSettingsRepository NotificationSettings => _notificationSettings ??= new NotificationSettingsRepository(_context);
        public IPayoutApprovalRepository PayoutApprovals => _payoutApprovals ??= new PayoutApprovalRepository(_context);
        public ISupportTicketRepository SupportTickets => _supportTickets ??= new SupportTicketRepository(_context);
        public ISupportTicketReplyRepository SupportTicketReplies => _supportTicketReplies ??= new SupportTicketReplyRepository(_context);
        public ICourseAnalyticsSnapshotRepository CourseAnalyticsSnapshots
            => _courseAnalyticsSnapshots ??= new CourseAnalyticsSnapshotRepository(_context);

        public IAppUserRepository AppUserRepository => _userRepository ??= new AppUserRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct = default)
            => _transaction = await _context.Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction is null) throw new InvalidOperationException("No active transaction.");
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction is null) return;
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
                await _transaction.DisposeAsync();

            await _context.DisposeAsync();
        }
    }
}

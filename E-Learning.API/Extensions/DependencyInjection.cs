namespace E_Learning.API.Extensions
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            // UnitOfWork
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            Services.AddScoped<IStageService, StageService>();
            Services.AddScoped<ILevelService, LevelService>();
            // Enrollment Services
           Services.AddScoped<IEnrollmentService, EnrollmentService>();
           Services.AddScoped<ILessonProgressService, LessonProgressService>();
           Services.AddScoped<IAssignmentService, AssignmentService>();
           Services.AddScoped<IAssignmentSubmissionService, AssignmentSubmissionService>();
           Services.AddScoped<IFileService, FileService>();
           //enralment Repositories
           Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
           Services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
          // notfications Services
           Services.AddScoped<INotificationService, NotificationService>();
           Services.AddScoped<INotificationSettingService, NotificationSettingService>();
            // Add services to the container.

            return Services;
        }
    }
}

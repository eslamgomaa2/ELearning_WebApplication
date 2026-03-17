using E_learning.API.Extensions;
using E_learning.Core.Entities.Identity;
using E_learning.Repository.Interceptors;
using E_Learning.API.Extensions;
using E_Learning.API.Hubs;  // NotificationHub
using E_Learning.API.Services;
using E_Learning.Core.Base;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Enrollments;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Core.Interfaces.Services.Academic;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Core.Interfaces.Services.Enrollments;
using E_Learning.Core.Interfaces.Services.Reviews_Certificates;
using E_Learning.Core.Repository;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories;
using E_Learning.Repository.Repositories.GenericesRepositories;
using E_Learning.Repository.Repositories.GenericesRepositories.Enrollments;
using E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions;
using E_Learning.Repository.Repositories.GenericesRepositories.Profile;
using E_Learning.Service.Contract;
using E_Learning.Service.Contract.Assignments;
using E_Learning.Service.Contract.Notifications;
using E_Learning.Service.Hubs;
using E_Learning.Service.Mapping;
using E_Learning.Service.Services;
using E_Learning.Service.Services.Academic;
using E_Learning.Service.Services.Academic.Stage;
using E_Learning.Service.Services.AssignmentService;
using E_Learning.Service.Services.Courses;
using E_Learning.Service.Services.Enrollments;
using E_Learning.Service.Services.LiveSessionServices;
using E_Learning.Service.Services.Notifications;
using E_Learning.Service.Services.Profiles;
using E_Learning.Service.Services.Reviews_Certificates;
using E_Learning.Service.Services.Schedule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace E_Learning.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // For Auditing Interceptor
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AuditInterceptor>();



            // DbContext Default
            builder.Services.AddDbContext<ELearningDbContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<AuditInterceptor>();

                var connectionString = builder.Environment.IsDevelopment()
                    ? builder.Configuration.GetConnectionString("DefaultConnection")
                    : builder.Configuration.GetConnectionString("DeployConnection");

                options.UseSqlServer(connectionString)
                       .AddInterceptors(interceptor);
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ELearningDbContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Auto Mapper
            builder.Services.AddAutoMapper(typeof(EnrollmentMappingProfile).Assembly);
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            builder.Services.AddTransient<ResponseHandler>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<IStudentService, StudentService>();


            builder.Services.AddScoped<ILiveSessionService, LiveSessionService>();
            builder.Services.AddScoped<ILiveSessionAttendeeService, LiveSessionAttendeeService>();

            builder.Services.AddScoped<ILiveSessionRepository, LiveSessionRepository>();
            builder.Services.AddScoped<ILiveSessionAttendeeRepository, LiveSessionAttendeeRepository>();
            builder.Services.AddScoped<IAdminProfileRepository, AdminProfileRepository>();
            builder.Services.AddScoped<IInstructorProfileRepository, InstructorProfileRepository>();
            builder.Services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
            builder.Services.AddScoped<ICertificateServices, CertificateServices>();


            //// Auto Mapper
            builder.Services.AddAutoMapper(typeof(EnrollmentMappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AssignmentProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(LiveSessionMappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AdminProfileMapping).Assembly);
            builder.Services.AddAutoMapper(typeof(AcademicMappingProfile).Assembly);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // builder.Services.AddAutoMapper(typeof(LiveSessionMappingProfile));
            // ResponseHandler
            builder.Services.AddTransient<ResponseHandler>();
            // Stage & Level Services
            builder.Services.AddScoped<IStageService, StageService>();
            builder.Services.AddScoped<ILevelService, LevelService>();
            // Enrollment Services
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<ILessonProgressService, LessonProgressService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<IAssignmentSubmissionService, AssignmentSubmissionService>();
            builder.Services.AddScoped<IFileService, FileService>();
            // Enrollment Repositories
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
            // Notifications Services
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationSettingService, NotificationSettingService>();

            builder.Services.AddScoped<INotificationHubService, NotificationHubService>();
            // CourseService
            builder.Services.AddScoped<ICourseContentService, CourseContentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            // AddApplicationServices
            builder.Services.AddScoped<INotificationHubService, NotificationHubService>();  // ← ضيف السطر ده
            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddSignalR();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:3000",   // React
                            "http://localhost:4200",   // Angular
                            "http://localhost:5173"    // Vite
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // AddApplicationServices (repositories + services)

            builder.Services.AddApplicationServices(builder.Configuration);



            // ══════════════════════════════════════════════════════
            // ═══════════ JWT Authentication Registration ══════════
            // ══════════════════════════════════════════════════════

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });

            builder.Services.AddSignalR();
               
            var app = builder.Build();

            // ─── Migration & Seeding ─────────────────────
            // await app.MigrateDatabaseAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();
            app.MapHub<LiveSessionHub>("/liveSessionHub");


            app.MapHub<NotificationHub>("/hubs/notifications");

            app.Run();
        }
    }
}
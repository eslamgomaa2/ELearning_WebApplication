
using E_learning.API.Extensions;
using E_learning.Core.Entities.Identity;
using E_learning.Repository.Interceptors;
using E_Learning.API.Extensions;
using E_Learning.API.Middleware;
using E_Learning.Core.Base;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Assessments.Assignments;
using E_Learning.Core.Interfaces.Repositories.Enrollments;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Core.Interfaces.Services.Enrollments;
using E_Learning.Core.Repository;
using E_Learning.Repository.Data;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories;
using E_Learning.Repository.Repositories.GenericesRepositories;
using E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Assignments;
using E_Learning.Repository.Repositories.GenericesRepositories.Enrollments;
using E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions;
using E_Learning.Service.Contract;
using E_Learning.Service.Contract.Assignments;
using E_Learning.Service.Mapping;
using E_Learning.Service.Mapping.Profile;
using E_Learning.Service.Services;
using E_Learning.Service.Services.AssignmentService;
using E_Learning.Service.Services.Courses;
using E_Learning.Service.Services.Enrollments;
using E_Learning.Service.Services.LiveSessionServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Learning.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AuditInterceptor>();

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

            // Auth
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            builder.Services.AddTransient<ResponseHandler>();

            builder.Services.AddScoped<ILiveSessionService, LiveSessionService>();
            builder.Services.AddScoped<ILiveSessionAttendeeService, LiveSessionAttendeeService>();
            
            builder.Services.AddScoped<ILiveSessionRepository, LiveSessionRepository>();
            builder.Services.AddScoped<ILiveSessionAttendeeRepository, LiveSessionAttendeeRepository>();
            //// Auto Mapper
            builder.Services.AddAutoMapper(typeof(EnrollmentMappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AssignmentProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(LiveSessionMappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AdminProfileMapping).Assembly);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // builder.Services.AddAutoMapper(typeof(LiveSessionMappingProfile));
            // ResponseHandler
            builder.Services.AddTransient<ResponseHandler>();

            // Enrollment Services
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<ILessonProgressService, LessonProgressService>();
            builder.Services.AddScoped<IAssignmentService,AssignmentService>();
            builder.Services.AddScoped<IAssignmentSubmissionService,AssignmentSubmissionService>();
            builder.Services.AddScoped<IFileService,FileService>();
            // Enrollment Repositories
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            builder.Services.AddScoped<IAssignmentSubmissionRepository, AssignmentSubmissionRepository>();
            // Add services to the container.
            builder.Services.AddScoped<ICourseService, CourseService>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.MigrateDatabaseAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}


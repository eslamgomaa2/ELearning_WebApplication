using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace E_Learning.Repository.Data.Seeding;

public static class AdminSeeding
{
    public static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager,
        ELearningDbContext context)  // ← أضفنا الـ DbContext
    {
        // ═══════════════════════════════════════════════════════
        // ADMIN ACCOUNT
        // ═══════════════════════════════════════════════════════
        const string adminEmail = "admin@elearning.com";
        const string adminPassword = "Admin@123456";

        var adminExists = await userManager.FindByEmailAsync(adminEmail);
        if (adminExists is null)
        {
            var admin = new ApplicationUser
            {
                FullName = "System Admin",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true,
                MemberSince = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Language = "en",
                TimeZone = "UTC"
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");

                // ── AdminProfile ──────────────────────────────
                var adminProfile = new AdminProfile
                {
                    AppUserId = admin.Id,
                    IsSuperAdmin = true,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Set<AdminProfile>().AddAsync(adminProfile);

                // ── Default NotificationSetting ───────────────
                var adminNotification = new NotificationSetting
                {
                    UserId = admin.Id
                    // كل الـ default values هتتاخد من الـ Entity تلقائياً
                };
                await context.Set<NotificationSetting>().AddAsync(adminNotification);

                var academicsetting = new AcademicSetting () { };
                await context.Set<AcademicSetting>().AddAsync(academicsetting);

                await context.SaveChangesAsync();
            }
        }

        // ═══════════════════════════════════════════════════════
        // INSTRUCTOR ACCOUNT
        // ═══════════════════════════════════════════════════════
        const string instructorEmail = "instructor@elearning.com";
        const string instructorPassword = "Instructor@123456";

        var instructorExists = await userManager.FindByEmailAsync(instructorEmail);
        if (instructorExists is null)
        {
            var instructor = new ApplicationUser
            {
                FullName = "Demo Instructor",
                UserName = instructorEmail,
                Email = instructorEmail,
                EmailConfirmed = true,
                IsActive = true,
                MemberSince = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Language = "en",
                TimeZone = "UTC"
            };

            var result = await userManager.CreateAsync(instructor, instructorPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(instructor, "Instructor");
                var instructorProfile = new InstructorProfile
                {
                    AppUserId = instructor.Id,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Set<InstructorProfile>().AddAsync(instructorProfile);

                // ── Default NotificationSetting ───────────────
                var instructorNotification = new NotificationSetting
                {
                    UserId = instructor.Id
                };
                await context.Set<NotificationSetting>().AddAsync(instructorNotification);

                await context.SaveChangesAsync();
            }
        }

        // ═══════════════════════════════════════════════════════
        // STUDENT ACCOUNT
        // ═══════════════════════════════════════════════════════
        const string studentEmail = "student@elearning.com";
        const string studentPassword = "Student@123456";

        var studentExists = await userManager.FindByEmailAsync(studentEmail);
        if (studentExists is null)
        {
            var student = new ApplicationUser
            {
                FullName = "Demo Student",
                UserName = studentEmail,
                Email = studentEmail,
                EmailConfirmed = true,
                IsActive = true,
                MemberSince = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Language = "en",
                TimeZone = "UTC"
            };

            var result = await userManager.CreateAsync(student, studentPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(student, "Student");
                var studnetProfile = new StudentProfile
                {
                    AppUserId = student.Id,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Set<StudentProfile>().AddAsync(studnetProfile);

                // ── Default NotificationSetting ───────────────
                var studentNotification = new NotificationSetting
                {
                    UserId = student.Id
                };
                await context.Set<NotificationSetting>().AddAsync(studentNotification);

                await context.SaveChangesAsync();
            }
        }
    }
}
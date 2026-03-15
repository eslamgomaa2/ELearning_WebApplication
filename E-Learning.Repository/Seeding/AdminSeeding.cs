using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Repository.Data.Seeding;

public static class AdminSeeding
{
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        // ═══════════════════════════════════════════════════════
        // ADMIN ACCOUNT
        // ═══════════════════════════════════════════════════════
        const string adminEmail    = "admin@elearning.com";
        const string adminPassword = "Admin@123456";

        var adminExists = await userManager.FindByEmailAsync(adminEmail);
        if (adminExists is null)
        {
            var admin = new ApplicationUser
            {
                FullName       = "System Admin",
                UserName       = adminEmail,
                Email          = adminEmail,
                EmailConfirmed = true,
                IsActive       = true,
                MemberSince    = DateTime.UtcNow,
                UpdatedAt      = DateTime.UtcNow,
                Language       = "en",
                TimeZone       = "UTC"
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // ═══════════════════════════════════════════════════════
        // INSTRUCTOR ACCOUNT
        // ═══════════════════════════════════════════════════════
        const string instructorEmail    = "instructor@elearning.com";
        const string instructorPassword = "Instructor@123456";

        var instructorExists = await userManager.FindByEmailAsync(instructorEmail);
        if (instructorExists is null)
        {
            var instructor = new ApplicationUser
            {
                FullName       = "Demo Instructor",
                UserName       = instructorEmail,
                Email          = instructorEmail,
                EmailConfirmed = true,
                IsActive       = true,
                MemberSince    = DateTime.UtcNow,
                UpdatedAt      = DateTime.UtcNow,
                Language       = "en",
                TimeZone       = "UTC"
            };

            var result = await userManager.CreateAsync(instructor, instructorPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(instructor, "Instructor");
        }
        // ═══════════════════════════════════════════════════════
        // Student ACCOUNT
        // ═══════════════════════════════════════════════════════

        var studentEmail = "student@elearning.com";
        var student = await userManager.FindByEmailAsync(studentEmail);
        if (student == null)
        {
            student = new ApplicationUser
            {
                FullName = "Demo Student",
                UserName = studentEmail,
                Email = studentEmail,
                EmailConfirmed = true,
                IsActive = true,
                MemberSince = DateTime.UtcNow,
            };
            await userManager.CreateAsync(student, "Student@123456");
            await userManager.AddToRoleAsync(student, "Student");
        }
    }
}
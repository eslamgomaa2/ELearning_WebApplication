using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Entities.Identity;
using E_Learning.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Data.Seeding
{
    public static class CourseSeeding
    {
        public static async Task SeedCoursesAsync(
            ELearningDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            if (await context.Courses.AnyAsync())
                return; // Already seeded

            // جلب الـ instructor اللي سييدتِه
            var instructor = await userManager.FindByEmailAsync("instructor@elearning.com");
            if (instructor == null)
                throw new Exception("Instructor not found. Seed the instructor first!");

            var courses = new List<Course>
            {
                new Course
                {
                    InstructorId = instructor.Id,
                    LevelId = null,
                    Title = "C# for Beginners",
                    Slug = "csharp-for-beginners",
                    Description = "Learn the basics of C# programming",
                    WhatYouWillLearn = "Variables, Loops, OOP, LINQ",
                    ThumbnailUrl = "https://picsum.photos/200",
                    Language = "English",
                    Price = 49.99m,
                    DurationInMinutes = 120,
                    Status = "Publish",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seeder"
                },
                new Course
                {
                    InstructorId = instructor.Id,
                    LevelId = null,
                    Title = "ASP.NET Core Web API",
                    Slug = "aspnetcore-webapi",
                    Description = "Build RESTful APIs using ASP.NET Core",
                    WhatYouWillLearn = "Controllers, EF Core, Authentication, Stripe integration",
                    ThumbnailUrl = "https://picsum.photos/200",
                    Language = "English",
                    Price = 79.99m,
                    DurationInMinutes = 180,
                    Status =  "Publish",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seeder"
                },
                new Course
                {
                    InstructorId = instructor.Id,
                    LevelId = null,
                    Title = "Entity Framework Core Deep Dive",
                    Slug = "efcore-deep-dive",
                    Description = "Master EF Core and Data Access Patterns",
                    WhatYouWillLearn = "DbContext, Migrations, Seeding, Performance",
                    ThumbnailUrl = "https://picsum.photos/200",
                    Language = "English",
                    Price = 59.99m,
                    DurationInMinutes = 150,
                    Status =  "Publish",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seeder"
                }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }
    }
}
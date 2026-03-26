using Bogus;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Entities.Courses;
using E_Learning.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

//public static class DbSeeder
//{
//    public static async Task SeedAsync(
//        ELearningDbContext context,
//        UserManager<ApplicationUser> userManager)
//    {
//        await context.Database.MigrateAsync();

//        //await SeedUsers(userManager);
//        //await SeedStages(context);
//        //await SeedLevels(context);
//        //await SeedCourses(context, userManager);
//    }

//    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
//    {
//        if (userManager.Users.Any())
//            return;

//        var faker = new Faker<ApplicationUser>()
//            .RuleFor(u => u.FullName, f => f.Name.FullName())
//            .RuleFor(u => u.Email, f => f.Internet.Email())
//            .RuleFor(u => u.UserName, (f, u) => u.Email)
//            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
//            .RuleFor(u => u.Language, "en")
//            .RuleFor(u => u.TimeZone, "UTC")
//            .RuleFor(u => u.IsActive, true)
//            .RuleFor(u => u.MemberSince, DateTime.UtcNow)
//            .RuleFor(u => u.UpdatedAt, DateTime.UtcNow);

//        var users = faker.Generate(15);

//        foreach (var user in users)
//        {
//            await userManager.CreateAsync(user, "Password123!");
//        }
//    }

//    private static async Task SeedStages(ELearningDbContext context)
//    {
//        if (await context.Stages.AnyAsync())
//            return;

//        var faker = new Faker<Stage>()
//            .RuleFor(s => s.Name, f => f.Name.FirstName())
//            .RuleFor(s => s.Description, f => f.Lorem.Sentence())
//            .RuleFor(s => s.OrderIndex, f => f.IndexFaker + 1)
//            .RuleFor(s => s.IsActive, true)
//            .RuleFor(s => s.CreatedAt, DateTime.UtcNow);

//        var stages = faker.Generate(3);

//        context.Stages.AddRange(stages);
//        await context.SaveChangesAsync();
//    }

//    //private static async Task SeedLevels(ELearningDbContext context)
//    //{
//    //    if (await context.Levels.AnyAsync())
//    //        return;

//    //    var stages = await context.Stages.ToListAsync();

//    //    var faker = new Faker<Level>()
//    //        .RuleFor(l => l.Name, f => $"Level {f.Random.Int(1, 5)}")
//    //        .RuleFor(l => l.OrderIndex, f => f.IndexFaker + 1)
//    //        .RuleFor(l => l.StageId, f => f.PickRandom(stages).Id)
//    //        .RuleFor(l => l.IsActive, true)
//    //        .RuleFor(l => l.CreatedAt, DateTime.UtcNow);

//    //    var levels = faker.Generate(10);

//    //    context.Levels.AddRange(levels);
//    //    await context.SaveChangesAsync();
//    //}

//    //private static async Task SeedCourses(
//    //    ELearningDbContext context,
//    //    UserManager<ApplicationUser> userManager)
//    //{
//    //    if (await context.Courses.AnyAsync())
//    //        return;

//    //    var levels = await context.Levels.ToListAsync();
//    //    var instructors = await userManager.Users.ToListAsync();

//    //    if (!instructors.Any())
//    //        return;

//    //    var faker = new Faker<Course>()
//    //        .RuleFor(c => c.Title, f => f.Name.FirstName())
//    //        .RuleFor(c => c.Slug, (f, c) => c.Title.Replace(" ", "-").ToLower())
//    //        .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
//    //        .RuleFor(c => c.WhatYouWillLearn, f => f.Lorem.Sentences(3))
//    //        .RuleFor(c => c.Price, f => f.Random.Decimal(10, 200))
//    //        .RuleFor(c => c.DurationInMinutes, f => f.Random.Int(60, 600))
//    //        .RuleFor(c => c.Language, "en")
//    //        .RuleFor(c => c.Status, "Published")
//    //        .RuleFor(c => c.IsActive, true)
//    //        .RuleFor(c => c.LevelId, f => f.PickRandom(levels).Id)
//    //        .RuleFor(c => c.InstructorId, f => f.PickRandom(instructors).Id)
//    //        .RuleFor(c => c.ThumbnailUrl, f => f.Image.PicsumUrl())
//    //        .RuleFor(c => c.CreatedAt, DateTime.UtcNow)
//    //        .RuleFor(c => c.UpdatedAt, DateTime.UtcNow);

//    //    var courses = faker.Generate(20);

//    //    context.Courses.AddRange(courses);
//    //    await context.SaveChangesAsync();
//    //}
//}
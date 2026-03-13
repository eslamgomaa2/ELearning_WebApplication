using E_Learning.Core.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationSettingConfiguration
    : IEntityTypeConfiguration<NotificationSetting>
{
    public void Configure(EntityTypeBuilder<NotificationSetting> builder)
    {
        builder.ToTable("NotificationSettings");
        builder.HasKey(ns => ns.Id);

        builder.HasIndex(ns => ns.UserId)
               .IsUnique()
               .HasDatabaseName("UQ_NotificationSetting_User");

        builder.Property(ns => ns.CourseAnnouncement)
               .HasDefaultValue(true);

        builder.Property(ns => ns.AssignmentReminder)
               .HasDefaultValue(true);

        builder.Property(ns => ns.ExamNotification)
               .HasDefaultValue(true);

        builder.Property(ns => ns.PlatformUpdates)
               .HasDefaultValue(true);

        builder.Property(ns => ns.InAppNotification)
               .HasDefaultValue(true);

        builder.Property(ns => ns.EmailNotification)
               .HasDefaultValue(true);

        builder.HasOne(ns => ns.User)
               .WithOne(u => u.NotificationSetting);

        builder.HasIndex(ns => ns.UserId)
               .IsUnique();
    }
}
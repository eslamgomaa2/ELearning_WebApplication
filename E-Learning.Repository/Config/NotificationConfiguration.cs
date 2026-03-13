using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
               .HasMaxLength(200).IsRequired();

        builder.Property(n => n.Body)
               .HasMaxLength(1000).IsRequired();

        builder.Property(n => n.Type)
               .HasConversion<string>()
               .HasMaxLength(30)
               .HasDefaultValue(NotificationType.General);

        builder.Property(n => n.IsRead)
               .HasDefaultValue(false);

        builder.Property(n => n.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(n => n.User)
               .WithMany(u => u.Notifications);



    }
}
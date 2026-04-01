using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Interfaces.Repositories.Notifications;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Notifications
{
    public class NotificationSettingsRepository
    : GenericRepository<NotificationSetting, int>, INotificationSettingsRepository
    {
        public NotificationSettingsRepository(ELearningDbContext context) : base(context) { }



        public Task<NotificationSetting?> GetByUserIdAsync(Guid userId, CancellationToken ct) 
            => QueryNoTracking().Include(x=>x.User).FirstOrDefaultAsync(s => s.UserId == userId);






    }
}

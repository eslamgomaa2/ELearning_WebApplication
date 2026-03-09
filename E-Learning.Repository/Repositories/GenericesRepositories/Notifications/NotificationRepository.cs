using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        public NotificationRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

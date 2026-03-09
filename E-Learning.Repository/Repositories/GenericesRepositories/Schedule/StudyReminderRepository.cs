using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Schedule
{
    public class StudyReminderRepository: IStudyReminderRepository
    {
        public StudyReminderRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

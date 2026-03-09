using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.AdminOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.AdminOperations
{
    public class CourseAnalyticsSnapshotRepository :ICourseAnalyticsSnapshotRepository
    {
        public CourseAnalyticsSnapshotRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

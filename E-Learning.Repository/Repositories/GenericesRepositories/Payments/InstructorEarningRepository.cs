using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Payments
{
    public class InstructorEarningRepository : IInstructorEarningRepository
    {
        public InstructorEarningRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

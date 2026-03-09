using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.AdminOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.AdminOperations
{
    public class PayoutApprovalRepository :IPayoutApprovalRepository
    {
        public PayoutApprovalRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

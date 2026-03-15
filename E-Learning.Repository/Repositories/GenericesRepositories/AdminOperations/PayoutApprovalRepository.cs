using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Interfaces.Repositories.AdminOperations;
using E_Learning.Repository.Data;
using System.Linq.Expressions;

namespace E_Learning.Repository.Repositories.GenericesRepositories.AdminOperations
{
    public class PayoutApprovalRepository
    : GenericRepository<PayoutApproval, int>, IPayoutApprovalRepository
    {
        public PayoutApprovalRepository(ELearningDbContext context) : base(context) { }
    }
}

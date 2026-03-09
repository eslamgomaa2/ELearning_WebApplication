using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Authentications
{
    public class UserSessionRepository : IUserSessionRepository
    {
        public UserSessionRepository(ELearningDbContext context)
        {
            Context = context;
        }

        public ELearningDbContext Context { get; }
    }
}

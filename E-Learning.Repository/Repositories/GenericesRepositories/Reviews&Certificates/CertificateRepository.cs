using E_Learning.Repository.Data;
using E_Learning.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Reviews_Certificates
{
    public class CertificateRepository : ICertificateRepository
    {
        public CertificateRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

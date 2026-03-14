using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Data
{
    public class ELearningDbContextFactory : IDesignTimeDbContextFactory<ELearningDbContext>
    {
        public ELearningDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ELearningDbContext>();

            optionsBuilder.UseSqlServer(
               "Server=AHMED-ATEF;Database=ELearning;Trusted_Connection=True;TrustServerCertificate=True");

            return new ELearningDbContext(optionsBuilder.Options, null);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace E_Learning.Repository.Data
{
    public class ELearningDbContextFactory : IDesignTimeDbContextFactory<ELearningDbContext>
    {
        public ELearningDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ELearningDbContext>();

            optionsBuilder.UseSqlServer(
                    "Server=.\\SQLEXPRESS;Database=ElearningWebApplicationDB;TrustServerCertificate=true;Trusted_Connection=true");
            return new ELearningDbContext(optionsBuilder.Options, null);
        }
    }
}

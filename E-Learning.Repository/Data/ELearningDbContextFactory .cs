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
                     "Server=DESKTOP-3D5BJO6;Database=ELearning;Trusted_Connection=True;TrustServerCertificate=True");
            return new ELearningDbContext(optionsBuilder.Options, null);
        }
    }
}

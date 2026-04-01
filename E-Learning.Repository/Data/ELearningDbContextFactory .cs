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
                    /*"Server = db45695.public.databaseasp.net; Database=db45695; User Id = db45695; Password=zE+2R9s%3@Dq; Encrypt=False; MultipleActiveResultSets=True;"*/);

            return new ELearningDbContext(optionsBuilder.Options, null);
        }
    }
}

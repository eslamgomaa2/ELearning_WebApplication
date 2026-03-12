using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Core.Interfaces.Repositories.Assessments.Assignments;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Assignments
{
    public class AssignmentSubmissionRepository : GenericRepository<AssignmentSubmission, int>,IAssignmentSubmissionRepository
    {
        public AssignmentSubmissionRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }
       

        public ELearningDbContext _context { get; }

        public async Task<AssignmentSubmission?> GetAssignmentSubmissionByIdWithAssimentData(int id)
        {
            return await _context.AssignmentSubmissions
                 .Include(a => a.Assignment)
                 .Include(a => a.Student)
          .FirstOrDefaultAsync(a => a.Id == id);


        }

        public async Task<IReadOnlyList<AssignmentSubmission>>  GetByAssignmentIdAsync(int assignmentId)
            {
                return await _context.AssignmentSubmissions
                    .Where(x => x.AssignmentId == assignmentId)
                    .ToListAsync();
            }

        public async Task<IReadOnlyList<AssignmentSubmission>>  GetByStudentIdAsync(Guid studentId)
        {
            return await _context.AssignmentSubmissions
                .Where(x => x.StudentId == studentId)
                .ToListAsync();
        }
    }
}

using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Assessments.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Assessments.Exams
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        public ExamQuestionRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

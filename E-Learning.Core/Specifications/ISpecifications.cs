using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get; }

        ICollection<Expression<Func<T, object>>> Includes { get; }

        List<string> IncludeStrings { get; }

        Expression<Func<T, object>> OrderBy { get; }

        Expression<Func<T, object>> OrderByDescending { get; }

        
        int Take { get; }

        int Skip { get; }
        
        bool IsPaginated { get; }
        bool AsNoTracking { get; }
    }
}

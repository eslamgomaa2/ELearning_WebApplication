using E_Learning.Core.Base;
using E_Learning.Core.Interfaces;
using System.Linq.Expressions;

namespace E_Learning.Core.Specifications;

public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
{
    #region Criteria (WHERE clause)

    /// <summary>
    /// The WHERE condition for filtering entities
    /// </summary>
    public Expression<Func<T, bool>> Criteria { get; private set; } = null;
    public bool AsNoTracking { get; private set; }


    /// <summary>
    /// Constructor for specifications WITH filtering criteria
    /// </summary>
    /// <param name="_Criteria">The filter expression (e.g., p => p.Price > 100)</param>
    protected BaseSpecification(Expression<Func<T, bool>> _Criteria)
    {
        Criteria = _Criteria;
    }

    /// <summary>
    /// Constructor for specifications WITHOUT filtering criteria (get all)
    /// </summary>
    protected BaseSpecification()
    {
    }

    #endregion

    #region Includes (Navigation Properties)

    /// <summary>
    /// Collection of navigation properties to include via eager loading
    /// </summary>
    public ICollection<Expression<Func<T, object>>> Includes { get; private set; } = [];
    public List<string> IncludeStrings { get; } = new();

    /// <summary>
    /// Adds a navigation property to be included in the query
    /// </summary>
    /// <param name="includeExpression">Navigation property expression</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);

    }

    // IncludeStrings 
    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
    #endregion

    #region Sorting (ORDER BY)

    /// <summary>
    /// Expression for ascending sort order
    /// </summary>
    public Expression<Func<T, object>> OrderBy { get; private set; }

    /// <summary>
    /// Expression for descending sort order
    /// </summary>
    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    /// <summary>
    /// Sets ascending sort order
    /// </summary>
    /// <param name="orderByExpression">Sort expression</param>
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Sets descending sort order
    /// </summary>
    /// <param name="orderByDescExpression">Sort expression</param>
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    #endregion

    #region Pagination (SKIP/TAKE)

    /// <summary>
    /// Number of records to take (page size)
    /// </summary>
    public int Take { get; private set; }

    /// <summary>
    /// Number of records to skip (offset)
    /// </summary>
    public int Skip { get; private set; }

    /// <summary>
    /// Indicates if pagination is enabled
    /// </summary>
    public bool IsPaginated { get; private set; }

    /// <summary>
    /// Enables pagination and calculates skip/take values
    /// </summary>
    /// <param name="pageIndex">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    protected void ApplyPagination(int pageIndex, int pageSize)
    //^^^^^^^^ Changed from public to protected
    {
        IsPaginated = true;
        Take = pageSize;
        Skip = (pageIndex - 1) * pageSize;
    }

    #endregion


    protected virtual void ApplyNoTracking()
    {
        AsNoTracking = true;
    }

    // Method to disable NoTracking
    protected virtual void DisableNoTracking()
    {
        AsNoTracking = false;
    }
}
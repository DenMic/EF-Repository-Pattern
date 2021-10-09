using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF_Repository_Pattern.Base
{
    internal class BaseRepository<TModel> where TModel : class, new()
    {
        protected readonly DbContext _context;
        private readonly bool _enableTracking;
        private readonly bool _enableSplitQuery;

        public BaseRepository(DbContext context, bool enableTracking = false, bool enableSplitQuery = false)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _enableTracking = enableTracking;
            _enableSplitQuery = enableSplitQuery;
        }

        protected IQueryable<TModel> GetDbSet()
        {
            var queryable = _context.Set<TModel>().AsQueryable();

            if(_enableTracking) queryable = queryable.AsNoTracking();

            // if(_enableSplitQuery) queryable = queryable.AsSplitQuery();

            return queryable;
        }

        protected IQueryable<TModel> SetInclude(IQueryable<TModel> query, IEnumerable<Expression<Func<TModel, object>>> includes)
        {
            if (includes != null && includes.Count() > 0)
            {
                foreach (var inc in includes)
                    query = query.Include(inc);
            }

            return query;
        }

        protected IQueryable<TModel> SetWhere(IQueryable<TModel> query, Expression<Func<TModel, bool>> predicate)
        {
            if (predicate != null) 
                query = query.Where(predicate);

            return query;
        }

        protected IQueryable<TModel> SetOrderBy(IQueryable<TModel> query, Expression<Func<TModel, object>> orderBy)
        {
            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return query;
        }

        protected IQueryable<TModel> SetPaging(IQueryable<TModel> query, int? pageIndex, int? pageSize)
        {
            if (pageIndex != null && pageSize != null)
            {
                query = query
                    .Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else if (pageSize != null)
            {
                query = query
                    .Take(pageSize.Value);
            }

            return query;
        }

        protected IQueryable<TModel> GenerateQueryExpression(
            Expression<Func<TModel, bool>> predicate, 
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderByFunc, 
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc)
        {
            var query = GetDbSet();

            if (includesFunc != null)
                query = includesFunc(query);

            query = SetWhere(query, predicate);

            if (orderByFunc != null)
                query = orderByFunc(query);

            return query;
        }
    }
}

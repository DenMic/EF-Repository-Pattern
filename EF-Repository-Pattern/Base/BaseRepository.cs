using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

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
    }
}

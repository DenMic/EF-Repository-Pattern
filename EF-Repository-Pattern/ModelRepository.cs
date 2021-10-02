using EF_Repository_Pattern.Base;
using EF_Repository_Pattern.Interface;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EF_Repository_Pattern
{
    internal class ModelRepository<TModel> : BaseRepository<TModel>, IModelRepository<TModel> where TModel : class, new()
    {
        public ModelRepository(DbContext dbContext,
            bool enableTracking = false,
            bool enableSplitQuery = false) : base(dbContext, enableTracking, enableSplitQuery)
        { }

        public async Task<IEnumerable<TModel>> GetListModelsAsync(
            Expression<Func<TModel, bool>> predicate = null, 
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderByFunc = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc = null, 
            int? pageSize = null, int? pageIndex = null
        )
        {
            var query = GetDbSet();

            if (includesFunc != null)
                query = includesFunc(query);

            query = SetWhere(query, predicate);

            if(orderByFunc != null)
                query = orderByFunc(query);

            query = SetPaging(query, pageIndex, pageSize);

            return await query.ToListAsync();
        }

        public async Task<TModel> GetFirstModelAsync(
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc = null)
        {
            var query = GetDbSet();

            if (includesFunc != null)
                query = includesFunc(query);

            query = SetWhere(query, predicate);

            return await query.FirstOrDefaultAsync();
        }

        #region Insert, Remove, Update

        public async Task<TModel> AddModelAsync(TModel model)
        {
            var entry = await _context.AddAsync(model);
            return entry.Entity;
        }

        public TModel RemoveModel(TModel model)
        {
            var entry = _context.Remove(model);
            return entry.Entity;
        }

        public TModel UpdateModel(TModel model)
        {
            var entry = _context.Update(model);
            return entry.Entity;
        }

        public async Task AddModelsAsync(params TModel[] models)
        {
            await _context.AddRangeAsync(models);
        }

        public void RemoveModels(params TModel[] models)
        {
            _context.RemoveRange(models);
        }

        public void UpdateModels(params TModel[] models)
        {
            _context.Update(models);
        }

        #endregion
    }
}

﻿using EF_Repository_Pattern.Base;
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

        /// <summary>
        /// Returns the list that matches the predicate entered
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByFunc"></param>
        /// <param name="includesFunc"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>IEnumerable<TModel></returns>
        public async Task<IEnumerable<TModel>> GetListModelsAsync(
            Expression<Func<TModel, bool>> predicate = null, 
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderByFunc = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc = null, 
            int? pageSize = null, 
            int? pageIndex = null
        )
        {
            var query = GenerateQueryExpression(predicate, orderByFunc, includesFunc);
            query = SetPaging(query, pageIndex, pageSize);

            return await query.ToListAsync();
        }

        /// <summary>
        /// It takes the first pattern found based on the predicate entered.
        /// If it finds nothing, it returns null
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includesFunc"></param>
        /// <returns>TModel</returns>
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

        /// <summary>
        /// Returns the pattern that matches the passed key. 
        /// The Model must inherit from the IBasePropertyKey<TKey> Interface.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns>TModel</returns>
        public async Task<TModel> GetModelByKeyAsync<TKey>(TKey id)
        {
            if (!typeof(IBasePropertyKey<TKey>).IsAssignableFrom(typeof(TModel)))
                throw new ArgumentException($"The class does not inherit from the IBasePropertyKey<TKey> interface");

            var query = (IQueryable<IBasePropertyKey<TKey>>)GetDbSet();

            return (TModel)await query.Where(x => EqualityComparer<TKey>.Default.Equals(x.Id, id)).SingleOrDefaultAsync();
        }

        #region Insert, Remove, Update

        /// <summary>
        /// Add Model in the context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TModel> AddModelAsync(TModel model)
        {
            var entry = await _context.AddAsync(model);
            return entry.Entity;
        }

        /// <summary>
        /// Remove Model in the context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TModel RemoveModel(TModel model)
        {
            var entry = _context.Remove(model);
            return entry.Entity;
        }

        /// <summary>
        /// Set Model to Update state in context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TModel UpdateModel(TModel model)
        {
            var entry = _context.Update(model);
            return entry.Entity;
        }

        /// <summary>
        /// Add a range of Model in the context
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task AddModelsAsync(params TModel[] models)
        {
            await _context.AddRangeAsync(models);
        }

        /// <summary>
        /// Reove a range of Models in the context
        /// </summary>
        /// <param name="models"></param>
        public void RemoveModels(params TModel[] models)
        {
            _context.RemoveRange(models);
        }

        /// <summary>
        /// Update a range of Models in the context
        /// </summary>
        /// <param name="models"></param>
        public void UpdateModels(params TModel[] models)
        {
            _context.Update(models);
        }

        #endregion
    }
}

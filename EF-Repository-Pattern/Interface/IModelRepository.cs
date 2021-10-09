
using EF_Repository_Pattern.Model;

using Microsoft.EntityFrameworkCore.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EF_Repository_Pattern.Interface
{
    public interface IModelRepository<TModel> where TModel : class, new()
    {
        Task<TModel> AddModelAsync(TModel model);
        TModel RemoveModel(TModel model);
        TModel UpdateModel(TModel model);
        Task AddModelsAsync(params TModel[] models);
        void RemoveModels(params TModel[] models);
        void UpdateModels(params TModel[] models);

        Task<IEnumerable<TModel>> GetListModelsAsync(
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderByFunc = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc = null,
            int? skip = null,
            int? take = null
        );

        Task<TModel> GetModelByKeyAsync<TKey>(TKey id);

        Task<TModel> GetFirstModelAsync(
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc = null);

        Task<PagerModel<TModel>> GetPagerModelAsync(
            int pageSize,
            int pageIndex,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderByFunc = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includesFunc = null
        );
    }
}
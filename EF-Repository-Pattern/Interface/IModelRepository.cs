using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EF_Repository_Pattern.Interface
{
    public interface IModelRepository<TModel> where TModel : class, new()
    {
        Task<TModel> GetFirstModelAsync(Expression<Func<TModel, bool>> predicate = null);
    }
}
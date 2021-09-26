using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace EF_Repository_Pattern.Interface
{
    public interface IRepositoryManager<TContext> where TContext : DbContext
    {
        IModelRepository<TModel> GenerateModelRepository<TModel>(bool enableTracking = false) where TModel : class, new();
        Task<int> SaveChangesAsync();
    }
}
using EF_Repository_Pattern.Interface;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF_Repository_Pattern.DependencyInjection;

public static class DIEFRepositoryPattern
{
    public static IServiceCollection AddEFRepositoryPattern<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        return services.AddScoped<IRepositoryManager<TContext>, RepositoryManager<TContext>>();
    }
}

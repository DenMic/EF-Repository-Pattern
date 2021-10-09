using System.Collections.Generic;

namespace EF_Repository_Pattern.Model
{
    public record PagerModel<TModel>(int PageIndex, int PageSize, long TotalCount, IEnumerable<TModel> ListModels) where TModel : class, new();
}

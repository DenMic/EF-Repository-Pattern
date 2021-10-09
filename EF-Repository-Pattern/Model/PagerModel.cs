using System.Collections.Generic;

namespace EF_Repository_Pattern.Model
{
    public class PagerModel<TModel> where TModel : class, new()
    {
        public int PageIndex { get; set; }
        public long TotalCount { get; set; }
        public IEnumerable<TModel> ListModels { get; set; }
    }
}

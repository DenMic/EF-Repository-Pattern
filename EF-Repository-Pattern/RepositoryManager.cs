﻿using EF_Repository_Pattern.Interface;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace EF_Repository_Pattern
{
    internal class RepositoryManager<TContext> : IRepositoryManager<TContext> where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public RepositoryManager(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IModelRepository<TModel> GenerateModelRepository<TModel>(bool enableTracking = false) where TModel : class, new()
        {
            return new ModelRepository<TModel>(_dbContext, enableTracking);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

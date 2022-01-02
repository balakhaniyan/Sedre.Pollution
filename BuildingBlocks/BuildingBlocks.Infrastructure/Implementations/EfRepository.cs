using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Implementations
{
    public sealed class EfRepository<T> : IRepository<T> where T : class , IEntity
    {
        private readonly DbContext _dbContext;

        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<T> Table => Set;
        private IQueryable<T> TableAsNoTracking => Set.AsNoTracking();
        private DbSet<T> Set => _dbContext.Set<T>();

        public async Task<T> GetAsync(Specification<T> spec)
        {
            var query = await ListAsync(spec);
        
            return query.FirstOrDefault();
        }

        public async Task Add(T entity)
        {
            await Set.AddAsync(entity);
        }

        public async Task AddList(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Add(entity);
            }
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        private Task DeleteAsync(T entity)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IList<T>> ListAsync(Specification<T> spec)
        {
            var query = await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec).ToListAsync();
            return query;
        }

    }
}
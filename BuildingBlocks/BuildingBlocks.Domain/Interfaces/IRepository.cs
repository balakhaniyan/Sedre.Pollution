using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task Add(T entity);
        Task AddList(IEnumerable<T> entities);
        Task<T> GetAsync(Specification<T> spec);
        Task DeleteAsync(IEnumerable<T> entities);
        Task<IList<T>> ListAsync(Specification<T> spec);
    }
}
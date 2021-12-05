using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace Sedre.Pollution.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
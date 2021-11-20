using System.Threading.Tasks;
using Sedre.Pollution.Domain.ProxyServices.Dto;

namespace Sedre.Pollution.Domain.ProxyServices
{
    public interface IAiInfo
    {
        Task<LastAiDataDto> GetLastData();
    }
}
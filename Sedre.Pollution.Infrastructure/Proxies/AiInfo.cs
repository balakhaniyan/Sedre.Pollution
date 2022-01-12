using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sedre.Pollution.Domain.ProxyServices;
using Sedre.Pollution.Domain.ProxyServices.Dto;

namespace Sedre.Pollution.Infrastructure.Proxies
{
    public class AiInfo : IAiInfo
    {
        private readonly HttpClient _client;
        private const string LastDataUrl = "http://185.73.115.95:5000/ground-base-data-ui";
        private const string DataUrl = "http://185.73.115.95:5000/ground-base-data-ui";
        
        public AiInfo()
        {
            var bypassSslHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(bypassSslHandler);
             
        }

        public async Task<LastAiDataDto> GetLastData()
        {
            var response = await _client.GetAsync(LastDataUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LastAiDataDto>(json);
            return result;
        }

        public async Task<LastAiDataDto> GetData(int year, int month, int day, int hour)
        {
            var input = "" + year + "-" + month + "-" + day + "-" + hour;
            var response = await _client.GetAsync(DataUrl+"?datetime="+input);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LastAiDataDto>(json);
            return result;
        }
        
        
        
    }
}
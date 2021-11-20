using System.Collections.Generic;

namespace Sedre.Pollution.Domain.ProxyServices.Dto
{
    public class LastAiDataDto
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public IList<AiIndicatorDto> indicators { get; set; }
    }
}
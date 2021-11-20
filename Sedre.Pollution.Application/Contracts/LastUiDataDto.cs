using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class LastUiDataDto
    {
        [Required] public int Date { get; set; }
        [Required] public int Time { get; set; }
        [Required] public IList<UiIndicatorDto> Indicators { get; set; }
    }
}
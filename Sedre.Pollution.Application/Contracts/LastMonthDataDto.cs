using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class LastMonthDataDto
    {
        [Required] public int Date { get; set; }
        [Required] public IList<AllDto> Indicators { get; set; }
    }
}
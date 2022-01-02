using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class LastDataDto
    {
        [Required] public int Date { get; set; }
        [Required] public int Time { get; set; }
        [Required] public IList<AllDto> Indicators { get; set; }
    }
}
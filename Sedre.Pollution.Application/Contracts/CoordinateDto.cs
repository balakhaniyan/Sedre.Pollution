using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class CoordinateDto
    {
        [Required] public int Date { get; set; }
        [Required] public int Time { get; set; }
        [Required] public AllDto Indicator { get; set; }
    }
}
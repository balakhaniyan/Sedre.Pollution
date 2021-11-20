using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class Pm25Dto : BaseIndicatorDto
    {
        [Required] public double Pm25 { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class Pm10Dto : BaseIndicatorDto
    {
        [Required] public double Pm10 { get; set; }
    }
}
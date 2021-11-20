using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class BaseIndicatorDto
    {
        [Required] public double Latitude { get; set; }
        [Required] public double Longitude { get; set; }
        public double All { get; set; }
    }
}
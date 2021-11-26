using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class MainMapDto
    {
        [Required] public double Latitude { get; set; }
        [Required] public double Longitude { get; set; }
        [Required] public string Color { get; set; }
    }
}
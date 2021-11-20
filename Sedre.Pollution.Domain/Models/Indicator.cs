using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain.Implementations;

namespace Sedre.Pollution.Domain.Models
{
    public class Indicator: TrackableEntity
    {
        [Required] public int Date { get; set; }
        [Required] public int Time { get; set; }
        
        [Required] public double Latitude { get; set; }
        [Required] public double Longitude { get; set; }
        
        [Required] public double O3 { get; set; }
        [Required] public double Co { get; set; }
        [Required] public double No2 { get; set; }
        [Required] public double So2 { get; set; }
        [Required] public double Pm10 { get; set; }
        [Required] public double Pm25 { get; set; }
    }
}
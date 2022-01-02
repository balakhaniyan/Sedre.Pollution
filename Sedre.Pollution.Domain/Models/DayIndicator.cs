using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain.Implementations;

namespace Sedre.Pollution.Domain.Models
{
    public class DayIndicator: Entity
    {
        [Required] public int Date { get; set; }
        
        [Required] public double ALatitude { get; set; }
        [Required] public double ALongitude { get; set; }        
        [Required] public double BLatitude { get; set; }
        [Required] public double BLongitude { get; set; }        
        [Required] public double CLatitude { get; set; }
        [Required] public double CLongitude { get; set; }        
        [Required] public double DLatitude { get; set; }
        [Required] public double DLongitude { get; set; }
        
        [Required] public double O3 { get; set; }
        [Required] public double Co { get; set; }
        [Required] public double No2 { get; set; }
        [Required] public double So2 { get; set; }
        [Required] public double Pm10 { get; set; }
        [Required] public double Pm25 { get; set; }
    }
}
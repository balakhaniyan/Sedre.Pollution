using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class MainMapDto
    {
        [Required] public double ALatitude { get; set; }
        [Required] public double ALongitude { get; set; }        
        [Required] public double BLatitude { get; set; }
        [Required] public double BLongitude { get; set; }        
        [Required] public double CLatitude { get; set; }
        [Required] public double CLongitude { get; set; }        
        [Required] public double DLatitude { get; set; }
        [Required] public double DLongitude { get; set; }
        [Required] public string Color { get; set; }
    }
}
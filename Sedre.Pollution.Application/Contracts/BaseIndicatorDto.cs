using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class BaseIndicatorDto
    {
        [Required] public double ALatitude { get; set; }
        [Required] public double ALongitude { get; set; }        
        [Required] public double BLatitude { get; set; }
        [Required] public double BLongitude { get; set; }        
        [Required] public double CLatitude { get; set; }
        [Required] public double CLongitude { get; set; }        
        [Required] public double DLatitude { get; set; }
        [Required] public double DLongitude { get; set; }
        public double All { get; set; }
    }
}
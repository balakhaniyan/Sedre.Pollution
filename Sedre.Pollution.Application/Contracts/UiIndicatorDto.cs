using System;
using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Application.Contracts;

namespace Sedre.Pollution.Application.Contracts
{
    public class UiIndicatorDto
    {

        [Required] public double ALatitude { get; set; }
        [Required] public double ALongitude { get; set; }        
        [Required] public double BLatitude { get; set; }
        [Required] public double BLongitude { get; set; }        
        [Required] public double CLatitude { get; set; }
        [Required] public double CLongitude { get; set; }        
        [Required] public double DLatitude { get; set; }
        [Required] public double DLongitude { get; set; }
        public double Co { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double Pm10 { get; set; }
        public double Pm25 { get; set; }
        public double So2 { get; set; }
    }
}
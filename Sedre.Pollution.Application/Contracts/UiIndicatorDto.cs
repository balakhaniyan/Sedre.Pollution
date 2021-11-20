using System;
using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Application.Contracts;

namespace Sedre.Pollution.Application.Contracts
{
    public class UiIndicatorDto
    {

        [Required] public double Latitude { get; set; }
        [Required] public double Longitude { get; set; }
        public double Co { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double Pm10 { get; set; }
        public double Pm25 { get; set; }
        public double So2 { get; set; }
    }
}
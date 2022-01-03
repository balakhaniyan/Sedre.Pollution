using System.Collections.Generic;
using Sedre.Pollution.Domain.Statics;

namespace Sedre.Pollution.Application.Contracts
{
    public class AllDto
    {
        public double Co { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double Pm10 { get; set; }
        public double Pm25 { get; set; }
        public double So2 { get; set; }
        public double All => Formula.DefineAll(new List<double>{Co,No2,O3,Pm10,Pm25,So2});
        public string Status => Formula.DefineStatus(All);
        public double ALatitude { get; set; }
        public double ALongitude { get; set; }        
        public double BLatitude { get; set; }
        public double BLongitude { get; set; }        
        public double CLatitude { get; set; }
        public double CLongitude { get; set; }        
        public double DLatitude { get; set; }
        public double DLongitude { get; set; }
        
        public int MinMoment { get; set; }
        public double MinValue { get; set; }
        public int MaxMoment { get; set; }
        public double MaxValue { get; set; }
    }
}
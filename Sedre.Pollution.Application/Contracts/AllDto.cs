namespace Sedre.Pollution.Application.Contracts
{
    public class AllDto : BaseIndicatorDto
    {
        public double Co { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double Pm10 { get; set; }
        public double Pm25 { get; set; }
        public double So2 { get; set; }
    }
}
using System.ComponentModel;

namespace Sedre.Pollution.Domain.Enums
{
    public enum IndicatorType
    {
        [Description("کلی")]
        All,
        [Description("O3_ppb")]
        O3,
        [Description("CO_ppm")]
        Co,
        [Description("NO2_ppb")]
        No2,
        [Description("SO2_ppb")]
        So2,
        [Description("PM_10")]
        Pm10,
        [Description("PM2_5")]
        Pm25
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Sedre.Pollution.Domain.Statics
{
    public static class Formula
    {

        public static double DefineAll(IEnumerable<double> indicators)
        {
            return indicators.Prepend(0.00).Max();
        }

        public static string DefineColor(double indicator)
        {
            var color = indicator switch
            {
                _ when indicator <= 10 => "00F0FF",
                _ when indicator <= 20 => "09F4BE",
                _ when indicator <= 30 => "12F87B",
                _ when indicator <= 40 => "1CFC39",
                _ when indicator <= 50 => "2CFF00",
                _ when indicator <= 60 => "5FFF00",
                _ when indicator <= 70 => "92FF00",
                _ when indicator <= 80 => "C5FF00",
                _ when indicator <= 90 => "F8FF00",
                _ when indicator <= 100 => "FBD600",
                _ when indicator <= 110 => "FBAC00",
                _ when indicator <= 120 => "FC8100",
                _ when indicator <= 130 => "FD5700",
                _ when indicator <= 140 => "FD4300",
                _ when indicator <= 150 => "FE3200",
                _ when indicator <= 160 => "FE2200",
                _ when indicator <= 170 => "FF1100",
                _ when indicator <= 180 => "FF0000",
                _ when indicator <= 190 => "FF000D",
                _ when indicator <= 200 => "EF0018",
                _ when indicator <= 210 => "FF0026",
                _ when indicator <= 220 => "FF0033",
                _ when indicator <= 230 => "FF003F",
                _ when indicator <= 240 => "FF004C",
                _ when indicator <= 250 => "F4006A",
                _ when indicator <= 260 => "E90088",
                _ when indicator <= 270 => "DE00A6",
                _ when indicator <= 280 => "D300C4",
                _ when indicator <= 290 => "C800E1",
                _ when indicator <= 300 => "BD00FF",
                _ when indicator <= 310 => "B601F0",
                _ when indicator <= 320 => "AF03E2",
                _ when indicator <= 330 => "A805D5",
                _ when indicator <= 340 => "A106C7",
                _ when indicator <= 350 => "9A08B9",
                _ when indicator <= 360 => "9309AB",
                _ when indicator <= 370 => "8C0B9D",
                _ when indicator <= 380 => "860D8E",
                _ when indicator <= 390 => "7F0E81",
                _ => "781073"
            };
            return $"#{color}";

            // var color = indicator switch
            // {
            //     _ when indicator <= 50 => Color.FromArgb(0, 228, 0),
            //     _ when indicator <= 100 => Color.FromArgb(255, 255, 0),
            //     _ when indicator <= 150 => Color.FromArgb(255, 126, 0),
            //     _ when indicator <= 200 => Color.FromArgb(255, 0, 0),
            //     _ when indicator <= 300 => Color.FromArgb(143, 63, 151),
            //     _ => Color.FromArgb(126, 0, 35)
            // };
            // return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static string DefineStatus(double indicator)
        {
            return indicator switch
            {
                _ when indicator <= 50 => "پاک",
                _ when indicator <= 100 => "سالم",
                _ when indicator <= 150 => "ناسالم برای گروه های حساس",
                _ => "ناسالم"
            };
        }
        
        public static List<List<double>> MakeCoordinatesList(
            double cALatitude, double cALongitude, 
            double cBLatitude, double cBLongitude, 
            double cCLatitude, double cCLongitude, 
            double cDLatitude, double cDLongitude)
        {
            return new List<List<double>>
            {
                new List<double> {cALongitude ,cALatitude },
                new List<double> {cBLongitude ,cBLatitude },
                new List<double> {cCLongitude ,cCLatitude },
                new List<double> {cDLongitude ,cDLatitude },
                new List<double> {cALongitude ,cALatitude }
            };
        }
        
    }
}
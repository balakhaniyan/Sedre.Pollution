using System.Collections.Generic;
using System.Linq;

namespace Sedre.Pollution.Domain.Statics
{
    public static class Formula
    {

        public static double AllFormula(IEnumerable<double> indicators)
        {
            return indicators.Prepend(0.00).Max();
        }

        public static string DefineColor(double indicator)
        {
            var color = indicator switch
            {
                _ when indicator <= 10 => "49955B",
                _ when indicator <= 20 => "5DA15D",
                _ when indicator <= 30 => "6EA458",
                _ when indicator <= 40 => "7EA654",
                _ when indicator <= 50 => "8FA850",
                _ when indicator <= 60 => "9FAB4C",
                _ when indicator <= 70 => "B0AD47",
                _ when indicator <= 80 => "C0AF43",
                _ when indicator <= 90 => "D1B23F",
                _ when indicator <= 100 => "E1B43A",
                _ when indicator <= 110 => "EEB437",
                _ when indicator <= 120 => "EDAE35",
                _ when indicator <= 130 => "ECA734",
                _ when indicator <= 140 => "EBA132",
                _ when indicator <= 150 => "EA9B30",
                _ when indicator <= 160 => "E9942F",
                _ when indicator <= 170 => "E88E2D",
                _ when indicator <= 180 => "E7872C",
                _ when indicator <= 190 => "E6812A",
                _ when indicator <= 200 => "E57A29",
                _ when indicator <= 210 => "E47427",
                _ when indicator <= 220 => "E2692A",
                _ when indicator <= 230 => "DF5E2D",
                _ when indicator <= 240 => "DC5230",
                _ when indicator <= 250 => "D94733",
                _ when indicator <= 260 => "D73C35",
                _ when indicator <= 270 => "D43138",
                _ when indicator <= 280 => "D1253B",
                _ when indicator <= 290 => "CE1A3E",
                _ when indicator <= 300 => "CC0F41",
                _ when indicator <= 310 => "C50E47",
                _ when indicator <= 320 => "BD104D",
                _ when indicator <= 330 => "B51253",
                _ when indicator <= 340 => "AD145A",
                _ when indicator <= 350 => "A51660",
                _ when indicator <= 360 => "9D1866",
                _ when indicator <= 370 => "961A6C",
                _ when indicator <= 380 => "8E1C73",
                _ when indicator <= 390 => "861E79",
                _ => "7E207F"
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
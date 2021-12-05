using System;
using System.Collections.Generic;
using System.Drawing;
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
                _ when indicator <= 50 => Color.FromArgb(0, 228, 0),
                _ when indicator <= 100 => Color.FromArgb(255, 255, 0),
                _ when indicator <= 150 => Color.FromArgb(255, 126, 0),
                _ when indicator <= 200 => Color.FromArgb(255, 0, 0),
                _ when indicator <= 300 => Color.FromArgb(143, 63, 151),
                _ => Color.FromArgb(126, 0, 35)
            };
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
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
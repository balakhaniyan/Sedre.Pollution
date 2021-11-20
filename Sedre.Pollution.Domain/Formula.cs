
using System.Collections.Generic;
using System.Linq;

namespace Sedre.Pollution.Domain
{
    public static class Formula
    {

        public static double AllFormula(IEnumerable<double> indicators)
        {
            return indicators.Prepend(0.00).Max();
        }
        
    }
}
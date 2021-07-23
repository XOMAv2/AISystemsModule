using System;
using System.Collections.Generic;
using System.Text;

namespace AISystemsModule.Extensions
{
    static class DoubleExtension
    {
        public static double Normalize(this double val, double min, double max)
        {
            return (val - min) / (max - min);
        }
    }
}

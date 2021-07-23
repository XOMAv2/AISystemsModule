using System;
using System.Collections.Generic;
using System.Text;

namespace AISystemsModule.Extensions
{
    static class Int32Extension
    {
        public static double Normalize(this int val, double min, double max)
        {
            return (val - min) / (max - min);
        }
    }
}

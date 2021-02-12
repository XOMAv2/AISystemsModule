using System;

namespace DataGenerator.Extensions
{
    static class RandomExtension
    {
        public static double NextDouble(this Random rand, double inclusiveMin, double exclusiveMax)
        {
            return rand.NextDouble() * (exclusiveMax - inclusiveMin) + inclusiveMin;
        }
    }
}

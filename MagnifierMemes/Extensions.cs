#region

using System;

#endregion

namespace MagnifierMemes
{
    public static class Extensions
    {
        public static float NextFloat(
            this Random random,
            double minValue,
            double maxValue)
        {
            return (float) (random.NextDouble() * (maxValue - minValue) + minValue);
        }
    }
}
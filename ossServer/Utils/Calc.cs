using System;

namespace ossServer.Utils
{
    public static class Calc
    {
        public static double RealRound(double mit, double mire)
        {
            if (mit >= 0)
                return Math.Truncate(mit / mire + 0.5d) * mire;
            return Math.Truncate(mit / mire - 0.5d) * mire;
        }

        public static decimal RealRound(decimal mit, decimal mire)
        {
            if (mit >= 0)
                return Math.Truncate(mit / mire + 0.5m) * mire;
            return Math.Truncate(mit / mire - 0.5m) * mire;
        }
    }
}

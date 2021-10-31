using System;

namespace TCCApp.Services
{
    public class DistanceService
    {
        public const double R = 6371;

        //Formula haversine
        public static double CompareDistance(double latStart, double lonStart, double latEnd, double lonEnd)
        {
            double t1 = latStart * Math.PI / 180;
            double t2 = latEnd * Math.PI / 180;
            double delta1 = (latEnd - latStart) * Math.PI / 180;
            double delta2 = (lonEnd - lonStart) * Math.PI / 180;

            double a = Math.Sin(delta1 / 2) * Math.Sin(delta1 / 2) +
                       Math.Cos(t1) * Math.Cos(t2) *
                       Math.Sin(delta2 / 2) * Math.Sin(delta2 / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c;

            return d;
        }
    }
}

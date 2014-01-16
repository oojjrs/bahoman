using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public static class MyMath
    {
        public static double DegreeToRadian(double degree)
        {
            return Math.PI / 180.0 * degree;
        }

        public static double RadianToDegree(double radian)
        {
            return 180.0 / Math.PI * radian;
        }
    }
}

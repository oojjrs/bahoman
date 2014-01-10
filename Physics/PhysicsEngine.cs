using System;
using System.Drawing;

namespace Physics
{
    public class PhysicsEngine
    {
        public static float GetDistance(Point startpoint, Point targetpoint)
        {
            return (float)Math.Sqrt(Math.Pow(startpoint.X - targetpoint.X, 2) + Math.Pow(startpoint.Y - targetpoint.Y, 2));
        }

    }
}

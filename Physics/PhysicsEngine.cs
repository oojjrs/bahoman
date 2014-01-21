using System;
using System.Drawing;

namespace Physics
{
    public class PhysicsEngine
    {
        public static float GetDistance(Vector3f startpoint, Vector3f targetpoint)
        {
            return (float)Math.Sqrt(Math.Pow(startpoint.X - targetpoint.X, 2) + Math.Pow(startpoint.Y - targetpoint.Y, 2) + Math.Pow(startpoint.Z - targetpoint.Z, 2));
        }
    }
}

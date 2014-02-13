using System;
using System.Drawing;

using Core;

namespace Physics
{
    public class PhysicsEngine
    {
        public static float GetDistance(Vector3f startpoint, Vector3f targetpoint)
        {
            return (float)Math.Sqrt(Math.Pow(startpoint.X - targetpoint.X, 2) + Math.Pow(startpoint.Y - targetpoint.Y, 2) + Math.Pow(startpoint.Z - targetpoint.Z, 2));
        }

        public static Vector3f GetIntersectLocation(Vector3f v1, Vector3f v2, Vector3f u1, Vector3f u2)
        {
            var l = (u2.Z - u1.Z) * (v2.X - v1.X) - (u2.X - u1.X) * (v2.Z - v1.Z);
            if (l < float.Epsilon)
                return new Vector3f();

            var x = ((u2.X - u1.X) * (v1.Z - u1.Z) - (u2.Z - u1.Z) * (v1.X - u1.X)) / l;
            var z = ((v2.X - v1.X) * (v1.Z - u1.Z) - (v2.Z - v1.Z) * (v1.X - u1.X)) / l;
            if (x < 0.0f || x > 1.0f || z < 0.0f || z > 1.0f)
                return new Vector3f();

            return new Vector3f(v1.X + x * (v2.X - v1.X), 0.0f, v1.Z + z * (v2.Z - v1.Z));
        }
    }
}

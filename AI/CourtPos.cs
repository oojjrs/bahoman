using System;
using System.Drawing;
using Core;
using Physics;

namespace AI
{
    public class CourtPos
    {
        // Note : (0, 0) = Center of Court
        private Vector3f pos = new Vector3f();

        public static readonly CourtPos Center = new CourtPos();

        public static CourtPos operator +(CourtPos l, CourtPos r)
        {
            return new CourtPos(l.pos + r.pos);
        }

        public static CourtPos operator -(CourtPos l, CourtPos r)
        {
            return new CourtPos(l.pos - r.pos);
        }

        public static CourtPos FromCoord(float x, float y, float z)
        {
            return new CourtPos(new Vector3f(x, y, z));
        }

        public static CourtPos FromVector(Vector3f v)
        {
            return new CourtPos(v);
        }

        public CourtPos()
        {
        }

        public CourtPos(Vector3f v)
        {
            pos = v;
        }

        public float DistanceTo(CourtPos target)
        {
            return Vector3f.Distance(pos, target.pos);
        }

        public Vector3f Location
        {
            get { return pos; }
        }

        public float X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public float Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        public float Z
        {
            get { return pos.Z; }
            set { pos.Z = value; }
        }
    }
}

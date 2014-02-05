using System;
using Core;
using Physics;

namespace AI
{
    public struct CourtPos
    {
        // Note : (0, 0) = Center of Court
        private Vector3f pos;

        public static readonly CourtPos Center = new CourtPos();

        public static CourtPos operator +(CourtPos l, CourtPos r)
        {
            return new CourtPos(l.pos + r.pos);
        }

        public static CourtPos operator -(CourtPos l, CourtPos r)
        {
            return new CourtPos(l.pos - r.pos);
        }

        public static CourtPos operator *(CourtPos l, float r)
        {
            return new CourtPos(l.pos * r);
        }

        public static CourtPos FromCoord(float x, float y, float z)
        {
            return new CourtPos(new Vector3f(x, y, z));
        }

        public static CourtPos FromVector(Vector3f v)
        {
            return new CourtPos(v);
        }

        public CourtPos(Vector3f v)
        {
            pos = v;
        }

        public float DistanceTo(CourtPos target)
        {
            return Vector3f.Distance(pos, target.pos);
        }

        public void RotateY(float radian)
        {
            this.X = (float)(this.X * Math.Cos(radian) - this.Z * Math.Sin(radian));
            this.Z = (float)(this.X * Math.Sin(radian) + this.Z * Math.Cos(radian));
        }

        public CourtPos Normalize
        {
            get
            {
                var dup = this;
                dup.pos.Normalize();
                return dup;
            }
        }

        public CourtPos InvertX
        {
            get
            {
                var dup = this;
                dup.X = -dup.X;
                return dup;
            }
        }

        public CourtPos InvertY
        {
            get
            {
                var dup = this;
                dup.Y = -dup.Y;
                return dup;
            }
        }

        public CourtPos InvertZ
        {
            get
            {
                var dup = this;
                dup.Z = -dup.Z;
                return dup;
            }
        }

        public float Length
        {
            get { return pos.Length(); }
        }

        public Vector3f Vector
        {
            get { return pos; }
            set { pos = value; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physics
{
    public class Vector3f
    {
        private float x = 0.0f;
        private float y = 0.0f;
        private float z = 0.0f;

        public override string ToString()
        {
            return String.Format("VECTOR3F(X{0}Y{1}Z{2})", x, y, z);
        }

        public Vector3f()
        {
        }

        public Vector3f(float x, float y, float z)
        {
            this.Set(x, y, z);
        }

        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Zero()
        {
            Zero(this);
        }

        public static void Zero(Vector3f v1)
        {
            v1.X = 0;
            v1.Y = 0;
            v1.Z = 0;
        }
        public static bool operator <(Vector3f v1, Vector3f v2)
        {
            return v1.Length() < v2.Length();
        }

        public static bool operator <=(Vector3f v1, Vector3f v2)
        {
            return v1.Length() <= v2.Length();
        }

        public static bool operator >(Vector3f v1, Vector3f v2)
        {
            return v1.Length() > v2.Length();
        }

        public static bool operator >=(Vector3f v1, Vector3f v2)
        {
            return v1.Length() >= v2.Length();
        }

        public static bool operator ==(Vector3f v1, Vector3f v2)
        {
            if (ReferenceEquals(v1, v2))
                return true;

            if ((object)v1 == null || (object)v2 == null)
                return false;

            return Math.Abs(v1.X - v2.X) <= float.Epsilon && Math.Abs(v1.Y - v2.Y) <= float.Epsilon;
        }

        public static bool operator !=(Vector3f v1, Vector3f v2)
        {
            return !(v1 == v2);
        }

        public static Vector3f operator +(Vector3f v1)
        {
            return new Vector3f(+v1.X, +v1.Y, +v1.Z);
        }

        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3f operator -(Vector3f v1)
        {
            return new Vector3f(-v1.X, -v1.Y, -v1.Z);
        }

        public static Vector3f operator -(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3f operator *(Vector3f v1, float s)
        {
            return new Vector3f(v1.X * s, v1.Y * s, v1.Z * s);
        }

        public static Vector3f operator *(float s, Vector3f v1)
        {
            return v1 * s;
        }

        public static Vector3f operator /(Vector3f v1, float s)
        {
            return new Vector3f(v1.X / s, v1.Y / s, v1.Z / s);
        }

        public Vector3f Reverse()
        {
            return new Vector3f(-x, -y, -z);
        }

        public float Length()
        {
            return (float)Math.Sqrt(LengthSqr());
        }

        public static float Length(Vector3f v1)
        {
            return (float)Math.Sqrt(LengthSqr(v1));
        }

        public float LengthSqr()
        {
            return x * x + y * y + z * z;
        }

        public static float LengthSqr(Vector3f v1)
        {
            return v1.X * v1.X + v1.Y * v1.Y + +v1.Z * v1.Z;
        }

        public float Distance(Vector3f v2)
        {
            return (float)Math.Sqrt(DistanceSqr(v2));
        }

        public static float Distance(Vector3f v1, Vector3f v2)
        {
            return (float)Math.Sqrt(DistanceSqr(v1, v2));
        }

        public float DistanceSqr(Vector3f v2)
        {
            float xDifference = v2.X - x;

            float yDifference = v2.Y - y;

            float zDifference = v2.Z - z;

            return xDifference * xDifference + yDifference * yDifference + zDifference * zDifference;
        }

        public static float DistanceSqr(Vector3f v1, Vector3f v2)
        {
            float xDifference = v2.X - v1.X;

            float yDifference = v2.Y - v1.Y;

            float zDifference = v2.Z - v1.Z;

            return xDifference * xDifference + yDifference * yDifference + zDifference * zDifference;
        }

        public void Truncate(float max)
        {
            if (Length() > max)
            {
                Normalize();

                x *= max;

                y *= max;

                z *= max;
            }
        }

        public void Normalize()
        {
            float length = Length();

            if (length > float.Epsilon)
            {
                X /= length;
                Y /= length;
                Z /= length;
            }
        }

        public static void Normalize(Vector3f v1)
        {
            float length = v1.Length();

            if (length > float.Epsilon)
            {
                v1.X /= length;
                v1.Y /= length;
                v1.Z /= length;
            }
        }

        public static Vector3f NormalizeRet(Vector3f v1)
        {
            Vector3f temp = new Vector3f(v1.X, v1.Y, v1.Z);

            Normalize(temp);

            return temp;
        }

        public override int GetHashCode()
        {
            return (int)((X + Y + Z) % int.MaxValue);
        }


        public override bool Equals(object obj)
        {
            Vector3f v2 = obj as Vector3f;

            if (v2 != null)
                return v2 == this;
            else
                return false;
        }

        public bool Equals(Vector3f v2)
        {
            return this == v2;
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }
    }
}

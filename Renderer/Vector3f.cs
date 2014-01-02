using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public class Vector3f
    {
        private float x = 0.0f;
        private float y = 0.0f;
        private float z = 0.0f;

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("VECTOR3F(X{0}Y{1}Z{2})", x, y, z).ToString();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public class ImageArgs
    {
        public float rx = 0.0f;	// rotate
        public float ry = 0.0f;
        public float rz = 0.0f;
        public float sx = 1.0f;	// scale
        public float sy = 1.0f;
        public float sz = 1.0f;
        public int px = 0;	// pos
        public int py = 0;
        public int pz = 0;
        public IImage image = null;

        public override string ToString()
        {
            return String.Format("ImageName(P({0},{1},{2})R({3},{4},{5})S({6},{7},{8}))",
                rx, ry, rz, sx, sy, sz, px, py, pz);
        }
    }
}

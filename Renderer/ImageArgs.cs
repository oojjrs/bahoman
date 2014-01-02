using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public class ImageArgs
    {
        private float rx = 0.0f;	// rotate
        private float ry = 0.0f;
        private float rz = 0.0f;
        private float sx = 1.0f;	// scale
        private float sy = 1.0f;
        private float sz = 1.0f;
        private int px = 0;	// pos
        private int py = 0;
        private int pz = 0;
        //IImage* image;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.AppendFormat("ImageName(P({0},{1},{2})R({3},{4},{5})S({6},{7},{8}))",
                rx, ry, rz, sx, sy, sz, px, py, pz);

            return s.ToString();
        }
    }
}

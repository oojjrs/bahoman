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
        private bool correctToCenter = false;
        private IImage image = null;

        public override string ToString()
        {
            string name;
            if (image == null)
                name = "NoImage";
            else
                name = image.GetIdentifier();
            return String.Format("{9}(P({0},{1},{2})R({3},{4},{5})S({6},{7},{8}),CC({10}))",
                px, py, pz, rx, ry, rz, sx, sy, sz, name, correctToCenter);
        }

        public ImageArgs()
        {
        }

        public ImageArgs(IImage image)
        {
            this.Image = image;
        }

        public void SetPos(int x, int y)
        {
            this.SetPos(x, y, 0);
        }

        public void SetPos(int x, int y, int z)
        {
            this.PosX = x;
            this.PosY = y;
            this.PosZ = z;
        }

        public void SetRotation(float radX, float radY)
        {
            this.SetRotation(radX, radY, 0.0f);
        }

        public void SetRotation(float radX, float radY, float radZ)
        {
            rx = radX;
            ry = radY;
            rz = radZ;
        }

        public void SetScale(float x, float y)
        {
            this.SetScale(x, y, 0.0f);
        }

        public void SetScale(float x, float y, float z)
        {
            sx = x;
            sy = y;
            sz = z;
        }

        public int PosX
        {
            get { return px; }
            set { px = value; }
        }

        public int PosY
        {
            get { return py; }
            set { py = value; }
        }

        public int PosZ
        {
            get { return pz; }
            set { pz = value; }
        }

        public float RotateX
        {
            get { return rx; }
            set { rx = value; }
        }

        public float RotateY
        {
            get { return ry; }
            set { ry = value; }
        }

        public float RotateZ
        {
            get { return rz; }
            set { rz = value; }
        }

        public float ScaleX
        {
            get { return sx; }
            set { sx = value; }
        }

        public float ScaleY
        {
            get { return sy; }
            set { sy = value; }
        }

        public float ScaleZ
        {
            get { return sz; }
            set { sz = value; }
        }

        public bool CorrectToCenter
        {
            get { return correctToCenter; }
            set { correctToCenter = value; }
        }

        public IImage Image
        {
            get { return image; }
            set { this.image = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;

namespace Renderer
{
    public class ImageArgs
    {
        private Vector3f rotate;
        private Vector3f scale = new Vector3f(1.0f, 1.0f, 1.0f);
        private Vector3f pos;
        private bool correctToCenter = false;
        private IImage image = null;

        public override string ToString()
        {
            string name;
            if (image == null)
                name = "NoImage";
            else
                name = image.GetIdentifier();
            return String.Format("{0}(P({1})R({2})S({3}),CC({4}))", name, pos, rotate, scale, correctToCenter);
        }

        public ImageArgs()
        {
        }

        public ImageArgs(IImage image)
        {
            this.Image = image;
        }

        public Vector3f Location
        {
            get { return pos; }
            set { pos = value; }
        }

        public float PosX
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public float PosY
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        public float PosZ
        {
            get { return pos.Z; }
            set { pos.Z = value; }
        }

        public Vector3f Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }

        public float RotateX
        {
            get { return rotate.X; }
            set { rotate.X = value; }
        }

        public float RotateY
        {
            get { return rotate.Y; }
            set { rotate.Y = value; }
        }

        public float RotateZ
        {
            get { return rotate.Z; }
            set { rotate.Z = value; }
        }

        public Vector3f Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float ScaleX
        {
            get { return scale.X; }
            set { scale.X = value; }
        }

        public float ScaleY
        {
            get { return scale.Y; }
            set { scale.Y = value; }
        }

        public float ScaleZ
        {
            get { return scale.Z; }
            set { scale.Z = value; }
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

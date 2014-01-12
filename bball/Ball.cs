using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    class Ball : Object
    {
        private Point pos = new Point();
        private IImage image = null;

        public override void OnDraw(IRenderer r)
        {
            var pt = Court.GetCoordinate(pos);
            var ia = new ImageArgs(image);
            ia.CorrectToCenter = true;
            ia.SetPos(pt.X, pt.Y);
            r.PutImage(ia);
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        public Point Location
        {
            get { return pos; }
            set { pos = value; }
        }
    }
}

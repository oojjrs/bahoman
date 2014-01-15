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
        private int oldTick = 0;
        private float boundInterpolation = 0.0f;

        public override void OnDraw(IRenderer r)
        {
            if (Environment.TickCount - oldTick > 100)
            {
                boundInterpolation = ((float)Math.Sin(Environment.TickCount) + 1.0f) / 4 + 0.3f;
                oldTick = Environment.TickCount;
            }

            var pt = Court.LogicalCoordToPhysicalCoord(pos);
            var ia = new ImageArgs(image);
            ia.CorrectToCenter = true;
            ia.SetPos(pt.X, pt.Y);
            ia.SetScale(boundInterpolation);
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

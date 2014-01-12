using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;
using Renderer;

namespace bball
{
    class Court : Object
    {
        private IImage image = null;

        public override void OnDraw(IRenderer r)
        {
            ImageArgs ia = new ImageArgs(image);
            r.PutImage(ia);
        }

        public static Point GetCoordinate(int x, int y)
        {
            return Court.GetCoordinate(new Point(x, y));
        }

        public static Point GetCoordinate(Point pt)
        {
            pt.X = pt.X + GlobalVariables.CourtWidth / 2;
            pt.Y = pt.Y + GlobalVariables.CourtHeight / 2;
            return pt;
        }

        public Court(IRenderer r)
        {
            this.SetImage(r.GetImage("res/court.png", new MyColor(), "court"));
        }

        public void SetImage(IImage image)
        {
            this.image = image;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace bball
{
    class Court
    {
        public static Point GetCoordinate(int x, int y)
        {
            var pt = new Point(x, y);
            pt.X = pt.X + GlobalVariables.CourtWidth / 2;
            pt.Y = pt.Y + GlobalVariables.CourtHeight / 2;
            return pt;
        }
    }
}

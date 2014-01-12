using System;
using System.Drawing;
using Core;

namespace bball
{
    class Position
    {
        public static Rectangle GetPlayerPosition(Point playerposition)
        {
            var pos = Court.GetCoordinate(playerposition.X, playerposition.Y);
            return new  Rectangle(pos.X - 15, pos.Y - 15, 30, 30);
        }
    }
}

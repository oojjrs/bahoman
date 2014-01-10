using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;
using Renderer;
using AI;

namespace bball
{
    class Player
    {
        private Point playerposition = new Point(0,0);
        private PlayerState currentState = new PlayerState();
        //private PlayerState nextState = new PlayerState();
        private Rectangle rc;
        private MyColor color;

        public Player(int x, int y, Color c)
        {
            var pos = Court.GetCoordinate(x, y);
            rc = new Rectangle(pos.X - 15, pos.Y - 15, 30, 30);
            color = new MyColor(c);
        }

        public void OnDraw(IRenderer r) // 나중에 인터페이스로
        {
            r.PutRect(rc.Left, rc.Top, rc.Right, rc.Bottom, color);
        }

        public void Thinking()
        {

        }

        public void Action()
        {
            if (currentState == PlayerState.Dribble)
            {
                    
            }
        }

        public Point PlayerPosition
        {
            get
            {
                var pt = rc.Location;
                pt.Offset(15, 15);
                return pt;
            }
        }
    }
}

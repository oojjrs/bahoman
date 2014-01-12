using System;
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
        private Point playerPosition = new Point(0,0);
        private PlayerState currentState = new PlayerState();
        private TeamType teamType = new TeamType();
        //private PlayerState nextState = new PlayerState();

        public Player(int x, int y, TeamType teamtype)
        {
            playerPosition = new Point(x, y);
            teamType = teamtype;
        }

        public void OnDraw(IRenderer r) // 나중에 인터페이스로
        {
            Rectangle rc = Position.GetPlayerPosition(playerPosition);
            MyColor color = new MyColor();
            if (teamType == TeamType.Home)
            {
                color = new MyColor(Color.Blue);
            }
            else if (teamType == TeamType.Away)
            {
                color = new MyColor(Color.Red);
            }
            else
            {
                color = new MyColor(Color.Black);
            }
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
                return playerPosition;
                //return GetPlayerPotion(playerposition);
                //var pt = rc.Location;
                //pt.Offset(15, 15);
                //return pt;
            }
        }
    }
}

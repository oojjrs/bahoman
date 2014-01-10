using AI;
using System;
using System.Drawing;

namespace bball
{
    class Player
    {
        public Player()
        {
            
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

        private Point playerposition = new Point(0,0);
        private PlayerState currentState = new PlayerState();
        private PlayerState nextState = new PlayerState();


        public Point PlayerPosition
        {
            get { return this.playerposition; }
            set { this.playerposition = value; }
        }
    }
}

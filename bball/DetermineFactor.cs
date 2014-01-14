using System;
using System.Drawing;
using AI;

namespace bball
{
    class DetermineFactor
    {
        private Point playerPosition = new Point();
        private PlayerState currentState = new PlayerState();
        private TeamType teamType = new TeamType();

        public Point PlayerPosition
        {
            get { return this.playerPosition; }
            set { this.playerPosition = value; }

        }
        public PlayerState CurrentState
        {
            get { return this.currentState; }
            set { this.currentState = value; }

        }
        public TeamType TeamType
        {
            get { return this.teamType; }
            set { this.teamType = value; }

        }
    }
}

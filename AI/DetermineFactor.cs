using System;
using System.Drawing;
using AI;

namespace AI
{
    class DetermineFactor
    {
        private Point playerPosition = new Point();
        private PlayerState currentState = new PlayerState();
        private TeamState teamState = new TeamState();

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
        public TeamState TeamState
        {
            get { return this.teamState; }
            set { this.teamState = value; }

        }
    }
}

using System;
using System.Drawing;
using AI;

namespace AI
{
    public class DetermineFactor
    {
        private Point playerPosition = new Point();
        private Point targetPosition = new Point();
        private PlayerState currentState = new PlayerState();
        private TeamState teamState = new TeamState();

        public Point PlayerPosition
        {
            get { return this.playerPosition; }
            set { this.playerPosition = value; }

        }

        public Point TargetPosition
        {
            get { return this.targetPosition; }
            set { this.targetPosition = value; }
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

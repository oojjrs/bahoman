using System;
using System.Drawing;
using AI;

namespace AI
{
    public class DetermineFactor
    {
        private CourtPos playerPosition = new CourtPos();
        private PlayerState currentState = new PlayerState();
        private TeamState teamState = new TeamState();
        private TargetInfo targetInfo = new TargetInfo();

        public CourtPos PlayerPosition
        {
            get { return this.playerPosition; }
            set { this.playerPosition = value; }

        }

        public TargetInfo TargetInfo
        {
            get { return targetInfo; }
            set { targetInfo = value; }
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

using System;
using System.Collections.Generic;

using Core;

namespace AI
{
    public class DetermineFactor
    {
        private CourtPos playerLocation = new CourtPos();
        private CourtPos ballLocation = new CourtPos();
        private List<CourtPos> teammateLocations = new List<CourtPos>();
        private PlayerState currentState = new PlayerState();
        private TeamState teamState = new TeamState();
        private TargetInfo targetInfo = new TargetInfo();
        
        public List<CourtPos> TeammateLocations
        {
            get { return this.teammateLocations; }
            set { this.teammateLocations = value; }
        }

        public CourtPos PlayerLocation
        {
            get { return this.playerLocation; }
            set { this.playerLocation = value; }

        }

        public CourtPos BallLocation
        {
            get { return this.ballLocation; }
            set { this.ballLocation = value; }

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

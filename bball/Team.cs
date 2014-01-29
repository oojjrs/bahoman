using System;
using AI;
using System.Collections.Generic;

namespace bball
{
    class Team
    {
        private TeamState teamState;
        private List<Player> players = new List<Player>();
        private CourtPos targetRingLocation;
        private Team away;

        public void AddPlayer(Player player)
        {
            player.Team = this;
            players.Add(player);
        }

        public List<Player> CurrentEntries
        {
            get { return this.players; }
        }

        public TeamState TeamState
        {
            get { return this.teamState; }
            set { this.teamState = value; }
        }

        public CourtPos TargetRingLocation
        {
            get { return targetRingLocation; }
            set { targetRingLocation = value; }
        }

        public Team Away
        {
            get { return away; }
            set { away = value; }
        }
    }
}

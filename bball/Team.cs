using System;
using AI;
using System.Collections.Generic;

namespace bball
{
    public enum TeamType
    {
        Home, Away
    }
    
    class Team
    {
        private TeamType teamType;
        private TeamState teamState;
        private List<Player> players = new List<Player>();

        public Team(TeamType teamtype)
        {
            teamType = teamtype;
        }

        public void AddPlayer(Player player)
        {
            player.Team = this;
            players.Add(player);
        }

        public List<Player> Players
        {
            get { return this.players; }
        }

        public TeamType TeamType
        {
            get { return this.teamType; }
            set { this.teamType = value; }

        }

        public TeamState TeamState
        {
            get { return this.teamState; }
            set { this.teamState = value; }

        }
    }
}

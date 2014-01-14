using System;

namespace bball
{
    public enum TeamType
    {
        Home, Away
    }
    
    public enum TeamState
    {
        Attack, Defence, StrategyTime
    }

    class Team
    {
        private TeamType teamType;
        private TeamState teamState;

        public Team(TeamType teamtype)
        {
            teamType = teamtype;
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

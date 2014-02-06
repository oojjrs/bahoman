using System;
using System.Collections.Generic;
using System.Linq;

using AI;

namespace bball
{
    struct TeamInfo
    {
        private UID id;
        private string name;
        private List<Player> players;

        public TeamInfo(UID id)
        {
            this.id = id;
            name = "";
            players = new List<Player>();
        }

        public UID ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Player> Players
        {
            get { return players; }
        }
    }

    class Team
    {
        private readonly TeamInfo teamInfo;
        private TeamState teamState;
        private CourtPos targetRingLocation;
        private Team away;
        private Dictionary<Position, Player> entries = new Dictionary<Position, Player>();
        private BaseCourt baseCourt;

        public override bool Equals(object obj)
        {
            var t = obj as Team;
            if ((object)t == null)
                return false;
            return this.ID == t.ID;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public Team(TeamInfo ti)
        {
            teamInfo = ti;
        }

        public bool Initialize(Game currentGame, Team away, BaseCourt baseCourt)
        {
            this.BaseCourt = baseCourt;
            this.TeamState = TeamState.LooseBall;
            this.Away = away;

            foreach (var p in this.AllPlayers)
                p.Clear();

            // Note : 선발 엔트리를 집어넣음
            this.ChooseRandomPlayerToEntry(Position.PointGuard);
            this.ChooseRandomPlayerToEntry(Position.ShootingGuard);
            this.ChooseRandomPlayerToEntry(Position.SmallFoward);
            this.ChooseRandomPlayerToEntry(Position.PowerFoward);
            this.ChooseRandomPlayerToEntry(Position.Center);

            foreach (var entry in entries)
            {
                entry.Value.CurrentGame = currentGame;
                entry.Value.Location = Court.CreateRandomPos();
                entry.Value.Visible = true;
            }
            return true;
        }

        public void AddPlayer(Player player)
        {
            if (teamInfo.Players.Contains(player))
                return;

            player.Team = this;
            teamInfo.Players.Add(player);
        }

        private void ChooseRandomPlayerToEntry(Position pos)
        {
            var ret = from p in teamInfo.Players where p.OriginalPosition == pos select p;
            if (ret.Count() == 0)
                ret = from p in teamInfo.Players where p.CurrentPosition == Position.Bench select p;

            if (ret.Count() == 0)
                throw new Exception("팀에 선수가 부족합니다");

            entries[pos] = ret.First();  // 무조건 첫번째 걸리는 선수로 일단 합니다 ^^;
            entries[pos].CurrentPosition = pos;
        }

        public CourtPos GetDefaultPositionalLocation(Position pos)
        {
            var loc = Court.GetDefaultPositionalLocation(pos);
            if (baseCourt == BaseCourt.Left)
                return loc.InvertX;
            else if (baseCourt == BaseCourt.Right)
                return loc;
            return CourtPos.Center;
        }

        public UID ID
        {
            get { return teamInfo.ID; }
        }

        public List<Player> CurrentEntries
        {
            get { return new List<Player>(entries.Values); }
        }

        public List<Player> AllPlayers
        {
            get { return teamInfo.Players; }
        }

        public TeamState TeamState
        {
            get { return this.teamState; }
            set { this.teamState = value; }
        }

        public CourtPos TargetRingLocation
        {
            get { return targetRingLocation; }
        }

        public Team Away
        {
            get { return away; }
            private set { away = value; }
        }

        public BaseCourt BaseCourt
        {
            get { return baseCourt; }
            set
            {
                baseCourt = value;
                if (baseCourt == BaseCourt.Left)
                {
                    targetRingLocation = Court.RingLocation;
                }
                else if (baseCourt == BaseCourt.Right)
                {
                    targetRingLocation = Court.RingLocation.InvertX;
                }
            }
        }
    }
}

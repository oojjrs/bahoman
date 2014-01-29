using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bball
{
    class TeamPool
    {
        private Dictionary<UID, Team> teams = new Dictionary<UID, Team>();

        public UID Add(string name)
        {
            var uid = UID.Create(UID.Type.Team);
            if (uid != UID.Unknown)
            {
                var ti = new TeamInfo(uid);
                ti.Name = name;
                teams[uid] = new Team(ti);
            }
            return uid;
        }

        public bool AddPlayer(UID tid, Player player)
        {
            Team t;
            if (teams.TryGetValue(tid, out t) == false)
                return false;

            t.AddPlayer(player);
            return true;
        }

        public List<Team> AllTeams
        {
            get { return new List<Team>(teams.Values); }
        }
    }
}

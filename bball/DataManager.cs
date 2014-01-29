using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AI;

namespace bball
{
    class DataManager
    {
        private PlayerPool pp = new PlayerPool();
        private TeamPool tp = new TeamPool();

        public bool BuildData()
        {
            {
                var tid = tp.Add("A팀");
                this.AddDummyPlayer(tid, "PG", "res/Player.png", Position.PointGuard);
                this.AddDummyPlayer(tid, "SG", "res/Player.png", Position.ShootingGuard);
                this.AddDummyPlayer(tid, "SF", "res/Player.png", Position.SmallFoward);
                this.AddDummyPlayer(tid, "PF", "res/Player.png", Position.PowerFoward);
                this.AddDummyPlayer(tid, "C", "res/Player.png", Position.Center);
            }

            {
                var tid = tp.Add("B팀");
                this.AddDummyPlayer(tid, "PG", "res/Player2.png", Position.PointGuard);
                this.AddDummyPlayer(tid, "SG", "res/Player2.png", Position.ShootingGuard);
                this.AddDummyPlayer(tid, "SF", "res/Player2.png", Position.SmallFoward);
                this.AddDummyPlayer(tid, "PF", "res/Player2.png", Position.PowerFoward);
                this.AddDummyPlayer(tid, "C", "res/Player2.png", Position.Center);
            }

            return true;
        }

        private void AddDummyPlayer(UID tid, string name, string imageSubPath, Position position)
        {
            var image = ImageFactory.Create(imageSubPath);
            var ai = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            ai.SetReporter(Log.Instance);

            var pid = pp.Add(name, new DateTime(), image, ai, position);
            if (tid != UID.Null)
                tp.AddPlayer(tid, pp.Get(pid));
        }

        public List<Team> AllTeams
        {
            get { return tp.AllTeams; }
        }
    }
}

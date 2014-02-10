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
                this.AddDummyPlayer(tid, "PG", "res/Player.png", Position.PointGuard, 1);
                this.AddDummyPlayer(tid, "SG", "res/Player.png", Position.ShootingGuard, 2);
                this.AddDummyPlayer(tid, "SF", "res/Player.png", Position.SmallFoward, 3);
                this.AddDummyPlayer(tid, "PF", "res/Player.png", Position.PowerFoward, 4);
                this.AddDummyPlayer(tid, "C", "res/Player.png", Position.Center, 5);
            }

            {
                var tid = tp.Add("B팀");
                this.AddDummyPlayer(tid, "PG", "res/Player2.png", Position.PointGuard, 1);
                this.AddDummyPlayer(tid, "SG", "res/Player2.png", Position.ShootingGuard, 2);
                this.AddDummyPlayer(tid, "SF", "res/Player2.png", Position.SmallFoward, 3);
                this.AddDummyPlayer(tid, "PF", "res/Player2.png", Position.PowerFoward, 4);
                this.AddDummyPlayer(tid, "C", "res/Player2.png", Position.Center, 5);
            }

            return true;
        }

        private void AddDummyPlayer(UID tid, string name, string imageSubPath, Position position, int backnumber)
        {
            var image = ImageFactory.Create(imageSubPath);
            var ai = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            ai.SetReporter(Log.Instance);

            var pid = pp.Add(name, new DateTime(), image, ai, position, backnumber);
            var player = pp.Get(pid);
            if (tid != UID.Null)
                tp.AddPlayer(tid, player);

            var fixedRandom = new Random(pid.GetHashCode());
            player.SetFactor("Sight", (float)fixedRandom.NextDouble());
            player.SetFactor("CloseShotAbility", (float)fixedRandom.NextDouble());
            player.SetFactor("MiddleShotAbility", (float)fixedRandom.NextDouble());
            player.SetFactor("ThreePointShotAbility", (float)fixedRandom.NextDouble());
        }

        public List<Team> AllTeams
        {
            get { return tp.AllTeams; }
        }
    }
}

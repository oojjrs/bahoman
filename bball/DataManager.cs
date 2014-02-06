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
                this.AddDummyPlayer(tid, "PG", "res/Player.png", Position.PointGuard, 1.0f,1);
                this.AddDummyPlayer(tid, "SG", "res/Player.png", Position.ShootingGuard, 0.8f,2);
                this.AddDummyPlayer(tid, "SF", "res/Player.png", Position.SmallFoward, 0.6f,3);
                this.AddDummyPlayer(tid, "PF", "res/Player.png", Position.PowerFoward, 0.4f,4);
                this.AddDummyPlayer(tid, "C", "res/Player.png", Position.Center, 0.2f,5);
            }

            {
                var tid = tp.Add("B팀");
                this.AddDummyPlayer(tid, "PG", "res/Player2.png", Position.PointGuard, 1.0f,1);
                this.AddDummyPlayer(tid, "SG", "res/Player2.png", Position.ShootingGuard, 0.8f,2);
                this.AddDummyPlayer(tid, "SF", "res/Player2.png", Position.SmallFoward, 0.6f,3);
                this.AddDummyPlayer(tid, "PF", "res/Player2.png", Position.PowerFoward, 0.4f,4);
                this.AddDummyPlayer(tid, "C", "res/Player2.png", Position.Center, 0.2f,5);
            }

            return true;
        }

        private void AddDummyPlayer(UID tid, string name, string imageSubPath, Position position, float sight, int backnumber)
        {
            var image = ImageFactory.Create(imageSubPath);
            var ai = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            ai.SetReporter(Log.Instance);

            // Note : sight는 현재 임시값이지만 디버깅을 위해 랜덤을 사용하지 않는다.
            var pid = pp.Add(name, new DateTime(), image, ai, position, sight,backnumber);
            if (tid != UID.Null)
                tp.AddPlayer(tid, pp.Get(pid));
        }

        public List<Team> AllTeams
        {
            get { return tp.AllTeams; }
        }
    }
}

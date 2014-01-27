using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AI;

namespace bball
{
    class Game
    {
        private Court court = null;
        private Team homeTeam = null;
        private Team awayTeam = null;

        public bool Initialize()
        {
            court = new Court();
            court.Image = ImageFactory.Create("res/court.png");
            court.CreateBall(ImageFactory.Create("res/Ball.png"));

            var p1 = new Player();
            p1.CurrentGame = this;
            p1.PlayerLocation = CourtPos.Center;
            p1.Image = ImageFactory.Create("res/Player.png");
            p1.AI = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            p1.AI.SetReporter(Log.Instance);

            var p2 = new Player();
            p2.CurrentGame = this;
            p2.PlayerLocation = CourtPos.FromCoord(200, 0, 100);
            p2.Image = ImageFactory.Create("res/Player.png");
            p2.AI = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            p2.AI.SetReporter(Log.Instance);

            homeTeam = new Team(TeamType.Home);
            homeTeam.TeamState = TeamState.LooseBall;
            homeTeam.AddPlayer(p1);
            homeTeam.AddPlayer(p2);

            awayTeam = new Team(TeamType.Away);
            awayTeam.TeamState = TeamState.LooseBall;
            awayTeam.AddPlayer(this.CreateAwayRandomPlayer());
            awayTeam.AddPlayer(this.CreateAwayRandomPlayer());
            awayTeam.AddPlayer(this.CreateAwayRandomPlayer());
            awayTeam.AddPlayer(this.CreateAwayRandomPlayer());
            awayTeam.AddPlayer(this.CreateAwayRandomPlayer());
            return true;
        }

        private Player CreateAwayRandomPlayer()
        {
            var player = new Player();
            player.CurrentGame = this;
            player.PlayerLocation = Court.CreateRandomPos();
            player.Image = ImageFactory.Create("res/Player2.png");
            player.AI = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            player.AI.SetReporter(Log.Instance);
            return player;
        }

        public Court Court
        {
            get { return court; }
        }

        public Ball Ball
        {
            get { return court.Ball; }
        }
    }
}

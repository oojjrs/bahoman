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

            homeTeam = new Team();
            homeTeam.TargetRingLocation = Court.RightGoalPos;
            homeTeam.TeamState = TeamState.LooseBall;
            homeTeam.AddPlayer(this.CreatePlayer("res/Player.png", CourtPos.FromCoord(150, 0, 150)));
            homeTeam.AddPlayer(this.CreatePlayer("res/Player.png", CourtPos.FromCoord(350, 0, 150)));
            homeTeam.AddPlayer(this.CreatePlayer("res/Player.png", Court.CreateRandomPos()));
            homeTeam.AddPlayer(this.CreatePlayer("res/Player.png", Court.CreateRandomPos()));
            homeTeam.AddPlayer(this.CreatePlayer("res/Player.png", Court.CreateRandomPos()));

            awayTeam = new Team();
            awayTeam.TargetRingLocation = Court.LeftGoalPos;
            awayTeam.TeamState = TeamState.LooseBall;
            awayTeam.AddPlayer(this.CreatePlayer("res/Player2.png", Court.CreateRandomPos()));
            awayTeam.AddPlayer(this.CreatePlayer("res/Player2.png", Court.CreateRandomPos()));
            awayTeam.AddPlayer(this.CreatePlayer("res/Player2.png", Court.CreateRandomPos()));
            awayTeam.AddPlayer(this.CreatePlayer("res/Player2.png", Court.CreateRandomPos()));
            awayTeam.AddPlayer(this.CreatePlayer("res/Player2.png", Court.CreateRandomPos()));

            homeTeam.Away = awayTeam;
            awayTeam.Away = homeTeam;
            return true;
        }

        public void SetTeamState(Team target, TeamState state)
        {
            target.TeamState = TeamState.Attack;
            target.Away.TeamState = TeamState.Defence;
        }

        private Player CreatePlayer(string imageSubPath, CourtPos location)
        {
            var player = new Player();
            player.CurrentGame = this;
            player.PlayerLocation = location;
            player.Image = ImageFactory.Create(imageSubPath);
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

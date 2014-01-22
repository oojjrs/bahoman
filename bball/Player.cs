using System;
using System.Drawing;

using Core;
using Renderer;
using AI;
using System.Windows.Forms;
using Physics;

namespace bball
{
    class Player : Object
    {
        private CourtPos playerPosition = CourtPos.Center;
        private PlayerState currentState = new PlayerState();
        private int lastThinkTick = 0;
        private Boolean hasBall = false;
        private Team team = null;
        //private PlayerState prevState = new PlayerState();
        private IImage image = null;
        private Court court = Court.Instance;

        #region From IDrawable

        public override void OnDraw(IRenderer r)
        {
            ImageArgs ia = new ImageArgs(image);
            ia.Location = Court.ToGlobalLocation(playerPosition).Location;
            ia.Scale = new Vector3f(0.5f, 0.5f, 0.5f);
            ia.CorrectToCenter = true;
            r.PutImage(ia);
        }

        public override void OnUpdate()
        {
            this.Thinking();
            this.Action();
        }

        #endregion

        public Player(CourtPos pos, TeamType teamType, IRenderer r)
        {
            if (teamType == TeamType.Home)
            {
                this.team = court.HomeTeam;
            }
            else
            {
                this.team = court.AwayTeam;
            }
            playerPosition = pos;
            this.SetImage(r.GetImage("res/Player.png", new MyColor(), "Player"));
        }

        public void SetImage(IImage image)
        {
            this.image = image;
        }

        private void Thinking()
        {
            if (Environment.TickCount - lastThinkTick > 10)
            {
                var factor = new DetermineFactor();
                factor.CurrentState = currentState;
                factor.PlayerPosition = playerPosition;
                var targetInfo = new TargetInfo();
                targetInfo.Position = Court.RightGoalPos;
                targetInfo.TargetType = TargetInfo.Type.Goal;
                factor.TargetInfo = targetInfo;
                factor.TeamState = court.HomeTeam.TeamState;
                currentState = PlayerAI.Determine(factor);
            }
        }

        private void Action()
        {
            if (currentState == PlayerState.Dribble)
            {
                Move(Court.RightGoalPos);
            }
            else if (currentState == PlayerState.Shoot)
            {
                court.Ball.TargetLocation = Court.RightGoalPos;
                court.Ball.CurrentState = Ball.State.Shooting;
            }
            else if (currentState == PlayerState.FindBall)
            {
                if (playerPosition.DistanceTo(court.Ball.Location) < 1)
                {
                    //MessageBox.Show("볼 잡았스");
                    hasBall = true;
                    SetState(PlayerState.Dribble);
                }
                else
                {
                    //볼 주우러 이동
                    this.Move(court.Ball.Location);
                }
            }
        }

        public void Move(CourtPos target)
        {
            var vDirect = target - playerPosition;
            vDirect.Location.Normalize();
            playerPosition = playerPosition + vDirect;

            if (hasBall)
            {
                court.Ball.Location = court.Ball.Location + vDirect;
            }
        }

        public void SetState(PlayerState playerstate)
        {
            currentState = playerstate;
            if (currentState == PlayerState.Dribble)
            {
                court.HomeTeam.TeamState = TeamState.Attack;
                court.Ball.CurrentState = Ball.State.Dribbling;
            }
        }

        public CourtPos PlayerPosition
        {
            get
            {
                return playerPosition;
            }
        }

        public Boolean HasBall
        {
            get
            {
                return hasBall;
            }
        }
    }
}

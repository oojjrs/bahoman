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
        //private Team team = new Team();
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

        public Player(CourtPos pos, Team team, IRenderer r)
        {
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
                factor.TeamState = TeamState.LooseBall;
                currentState = PlayerAI.Determine(factor);
            }
        }

        private void Action()
        {
            if (currentState == PlayerState.Dribble)
            {
                playerPosition = playerPosition + CourtPos.FromCoord(1, 0, 0);
            }
            else if (currentState == PlayerState.Shoot)
            {
                MessageBox.Show("슛이다");
            }
            else if (currentState == PlayerState.FindBall)
            {
                if (playerPosition.DistanceTo(court.GetBallPosition()) < 1)
                {
                    SetState(PlayerState.Dribble);
                }
                else
                {
                    //볼 주우러 이동
                    var vDirect = playerPosition - court.Ball.Location;
                    vDirect.Location.Normalize();
                    playerPosition = playerPosition + vDirect;
                }
            }
        }

        public void SetState(PlayerState playerstate)
        {
            currentState = playerstate;
            if (currentState == PlayerState.Dribble)
            {
                court.Ball.CurrentState = Ball.State.Bounding;
            }
        }

        public CourtPos PlayerPosition
        {
            get
            {
                return playerPosition;
            }
        }
    }
}

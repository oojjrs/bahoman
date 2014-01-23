using System;

using Core;
using Renderer;
using AI;
using System.Windows.Forms;
using Physics;
using System.Collections.Generic;

namespace bball
{
    class Player : Object
    {
        private CourtPos playerLocation = null;
        private PlayerState currentState = new PlayerState();
        private int lastThinkTick = 0;
        private Boolean hasBall = false;
        private Team team = null;
        //private PlayerState prevState = new PlayerState();
        private IImage image = null;
        private Court court = Court.Instance;
        private IPlayerAIType ai = null;

        #region From IDrawable

        public override void OnDraw(IRenderer r)
        {
            ImageArgs ia = new ImageArgs(image);
            ia.Location = Court.ToGlobalLocation(playerLocation).Location;
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

        private void Thinking()
        {
            if (Environment.TickCount - lastThinkTick > 10)
            {
                var factor = new DetermineFactor();
                factor.CurrentState = currentState;
                factor.PlayerLocation = playerLocation;

                var teammateLocations = new List<CourtPos>();

                foreach (var player in team.Players)
	            {
                    teammateLocations.Add(player.playerLocation);
	            }

                factor.TeammateLocations =  teammateLocations;
                var targetInfo = new TargetInfo();

                targetInfo.Position = Court.RightGoalPos;
                targetInfo.TargetType = TargetInfo.Type.Goal;
                
                factor.TargetInfo = targetInfo;
                factor.TeamState = team.TeamState;
                currentState = this.AI.Determine(factor).state;
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
            else if (currentState == PlayerState.Free)
            {
                Move(Court.RightGoalPos);
            }
            else if (currentState == PlayerState.FindBall)
            {
                if (playerLocation.DistanceTo(court.Ball.Location) < 1)
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
            var vDirect = target - playerLocation;
            vDirect.Location.Normalize();
            playerLocation = playerLocation + vDirect;

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
                team.TeamState = TeamState.Attack;
                court.Ball.CurrentState = Ball.State.Dribbling;
            }
        }

        public CourtPos PlayerLocation
        {
            get { return playerLocation; }
            set { playerLocation = value; }
        }

        public Boolean HasBall
        {
            get { return hasBall; }
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        public Team Team
        {
            get { return team; }
            set { team = value; }
        }

        public IPlayerAIType AI
        {
            get { return ai; }
            set { ai = value; }
        }
    }
}

using System;

using Core;
using Renderer;
using AI;
using Physics;

namespace bball
{
    class Player : Object
    {
        private CourtPos playerLocation = null;
        private PlayerState currentState = new PlayerState();
        private int lastThinkTick = 0;
        private Boolean hasBall = false;
        private Team team = null;
        private float speed = 1;
        private Vector3f diretion = null;
        //private PlayerState prevState = new PlayerState();
        private IImage image = null;
        private IPlayerAIType ai = null;
        private Game currentGame = null;

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
                var factor = new PropertyBag();

                switch (team.TeamState)
                {
                    case TeamState.Attack:
                        factor.AddPrimitive("TeamState.Attack", true);
                        switch (currentState)
                        {
                            case PlayerState.Dribble:
                                factor.AddPrimitive("PlayerState.Dribble", true);
                                factor.AddPrimitive("TargetInfo.Type.Goal", true);
                                factor.AddVector("PlayerLocation", this.PlayerLocation.Location);
                                factor.AddVector("BallLocation", team.TargetRingLocation.Location);
                                foreach (var p in team.Players)
                                    factor.AddVector("TeammateLocation", p.PlayerLocation.Location);
                                factor.AddPrimitive("CanShoot", this.GetShootingPoint(this.PlayerLocation, team.TargetRingLocation) > 80);
                                break;
                            case PlayerState.Shoot:
                                factor.AddPrimitive("PlayerState.Shoot", true);
                                break;
                            case PlayerState.Pass:
                                factor.AddPrimitive("PlayerState.Pass", true);
                                break;
                            case PlayerState.Rebound:
                                factor.AddPrimitive("PlayerState.Rebound", true);
                                break;
                            case PlayerState.Free:
                                factor.AddPrimitive("PlayerState.Free", true);
                                break;
                            case PlayerState.FindBall:
                                factor.AddPrimitive("PlayerState.FindBall", true);
                                break;
                        }
                        break;
                    case TeamState.Defence:
                        factor.AddPrimitive("TeamState.Defence", true);
                        break;
                    case TeamState.LooseBall:
                        factor.AddPrimitive("TeamState.LooseBall", true);
                        factor.AddVector("PlayerLocation", this.PlayerLocation.Location);
                        factor.AddVector("BallLocation", team.TargetRingLocation.Location);
                        foreach (var p in team.Players)
                            factor.AddVector("TeammateLocation", p.PlayerLocation.Location);
                        break;
                    case TeamState.StrategyTime:
                        factor.AddPrimitive("TeamState.StrategyTime", true);
                        break;
                }

                currentState = this.AI.Determine(factor).state;
            }
        }

        private void Action()
        {
            if (currentState == PlayerState.Dribble)
            {
                Move(team.TargetRingLocation);
            }
            else if (currentState == PlayerState.Shoot)
            {
                this.CurrentGame.Ball.TargetLocation = team.TargetRingLocation;
                this.CurrentGame.Ball.CurrentState = Ball.State.Shooting;
            }
            else if (currentState == PlayerState.Free)
            {
                Move(team.TargetRingLocation);
            }
            else if (currentState == PlayerState.FindBall)
            {
                if (playerLocation.DistanceTo(this.CurrentGame.Ball.Location) < 1)
                {
                    //MessageBox.Show("볼 잡았스");
                    hasBall = true;
                    SetState(PlayerState.Dribble);
                }
                else
                {
                    //볼 주우러 이동
                    this.Move(this.CurrentGame.Ball.Location);
                }
            }
            else if (currentState == PlayerState.Pass)
            {
                //MessageBox.Show("언제쯤");
                if (this.CurrentGame.Ball.CurrentState != Ball.State.Passing)
                {
                    hasBall = false;
                    var playerDistance = playerLocation.DistanceTo(team.TargetRingLocation);
                    foreach (var player in team.Players)
                    {
                        if (playerDistance > player.PlayerLocation.DistanceTo(team.TargetRingLocation))
                        {
                            this.CurrentGame.Ball.TargetLocation = player.PlayerLocation + CourtPos.FromVector(player.Direction) * 3;
                            this.CurrentGame.Ball.CurrentState = Ball.State.Passing;
                            currentState = PlayerState.Free;
                        }

                    }
                }
            }
        }

        public void Move(CourtPos target)
        {
            var dir = target - playerLocation;
            this.diretion = dir.Location;
            dir.Location.Normalize();
            playerLocation = playerLocation + dir;
            
            if (hasBall)
            {
                this.CurrentGame.Ball.CurrentState = Ball.State.Dribbling;
                this.CurrentGame.Ball.Location = this.CurrentGame.Ball.Location + dir;
            }
        }

        public void SetState(PlayerState playerstate)
        {
            currentState = playerstate;
            if (currentState == PlayerState.Dribble)
            {
                currentGame.SetTeamState(team, TeamState.Attack);
                this.CurrentGame.Ball.CurrentState = Ball.State.Dribbling;
            }
        }

        private int GetShootingPoint(CourtPos bp, CourtPos ep)
        {
            //슛을 쏠지 말지 결정하는 팩터들을 수치화
            float distancefromRing = bp.DistanceTo(ep);

            //일단 골대 근처에 있으면 100점으로 리턴
            if (60 > (int)distancefromRing)
            {
                return 100;
            }
            else
            {
                return 0;
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

        public  Vector3f Direction
        {
            get { return diretion; }
            set { diretion = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Game CurrentGame
        {
            get { return currentGame; }
            set { currentGame = value; }
        }
    }
}

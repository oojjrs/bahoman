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
                                factor.AddVector("BallLocation", Court.RightGoalPos.Location);
                                foreach (var p in team.Players)
                                    factor.AddVector("TeammateLocation", p.PlayerLocation.Location);
                                factor.AddPrimitive("CanShoot", this.GetShootingPoint(this.PlayerLocation, Court.RightGoalPos) > 80);
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
                        factor.AddVector("BallLocation", Court.RightGoalPos.Location);
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
                Move(Court.RightGoalPos);
            }
            else if (currentState == PlayerState.Shoot)
            {
                this.CurrentGame.Ball.TargetLocation = Court.RightGoalPos;
                this.CurrentGame.Ball.CurrentState = Ball.State.Shooting;
            }
            else if (currentState == PlayerState.Free)
            {
                Move(Court.RightGoalPos);
            }
            else if (currentState == PlayerState.FindBall)
            {
                if (playerLocation.DistanceTo(this.CurrentGame.Ball.Location) < 1)
                {
                    //MessageBox.Show("�� ��ҽ�");
                    hasBall = true;
                    SetState(PlayerState.Dribble);
                }
                else
                {
                    //�� �ֿ췯 �̵�
                    this.Move(this.CurrentGame.Ball.Location);
                }
            }
            else if (currentState == PlayerState.Pass)
            {
                //MessageBox.Show("������");
                if (this.CurrentGame.Ball.CurrentState != Ball.State.Passing)
                {
                    var playerDistance = playerLocation.DistanceTo(Court.RightGoalPos);
                    foreach (var player in team.Players)
                    {
                        if (playerDistance > player.PlayerLocation.DistanceTo(Court.RightGoalPos))
                        {
                            this.CurrentGame.Ball.TargetLocation = player.PlayerLocation;
                            this.CurrentGame.Ball.CurrentState = Ball.State.Passing;
                            SetState(PlayerState.Free);
                        }

                    }
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
                this.CurrentGame.Ball.CurrentState = Ball.State.Dribbling;
                this.CurrentGame.Ball.Location = this.CurrentGame.Ball.Location + vDirect;
            }
        }

        public void SetState(PlayerState playerstate)
        {
            currentState = playerstate;
            if (currentState == PlayerState.Dribble)
            {
                team.TeamState = TeamState.Attack;
                this.CurrentGame.Ball.CurrentState = Ball.State.Dribbling;
            }
        }

        private int GetShootingPoint(CourtPos bp, CourtPos ep)
        {
            //���� ���� ���� �����ϴ� ���͵��� ��ġȭ
            float distancefromRing = bp.DistanceTo(ep);

            //�ϴ� ��� ��ó�� ������ 100������ ����
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

        public Game CurrentGame
        {
            get { return currentGame; }
            set { currentGame = value; }
        }
    }
}

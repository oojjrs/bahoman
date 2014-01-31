using System;

using Core;
using Renderer;
using AI;
using Physics;
using System.Windows.Forms;

namespace bball
{
    struct PlayerInfo
    {
        private UID id;
        private string name;
        private DateTime birthday;
        private IImage image;
        private IPlayerAIType ai;
        private Position position;

        public PlayerInfo(UID id)
        {
            this.id = id;
            name = "";
            birthday = new DateTime();
            image = null;
            ai = null;
            position = Position.Bench;
        }

        public UID ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        public IPlayerAIType AI
        {
            get { return ai; }
            set { ai = value; }
        }

        public Position Position
        {
            get { return position; }
            set { position = value; }
        }
    }

    class Player : Object
    {
        private readonly PlayerInfo playerInfo;
        private CourtPos playerLocation;
        private PlayerState currentState = new PlayerState();
        private int lastThinkTick = 0;
        private Boolean hasBall = false;
        private Team team = null;
        private float speed = 1;
        private Vector3f diretion;
        //private PlayerState prevState = new PlayerState();
        private Game currentGame = null;
        private Position currentPosition = Position.Bench;
        private CourtPos targetLocation;

        #region From IDrawable

        public override void OnDraw(IRenderer r)
        {
            var loc = Court.ToGlobalLocation(this.PlayerLocation);
            ImageArgs ia = new ImageArgs(playerInfo.Image);
            ia.Location = loc.Location;
            ia.Scale = new Vector3f(0.5f, 0.5f, 0.5f);
            ia.CorrectToCenter = true;
            r.PutImage(ia);

            var rc = new System.Drawing.Rectangle((int)loc.X, (int)loc.Z, 0, 0);
            using (var g = System.Drawing.Graphics.FromHwnd(r.GetHandle()))
            {
                var sizef = g.MeasureString(this.Name, OutputManager.DefaultFont);
                rc.Width = (int)sizef.Width + 1;
                rc.Height = (int)sizef.Height + 1;
                rc.Offset(-rc.Width / 2, (int)(rc.Height * 1.5));    // 위에서 이미지가 height/2 만큼 내려오므로 적당히 아래로 더 내린다.
            }

            var ta = TextArgs.Create(this.Name, OutputManager.DefaultFont);
            ta.Format = TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            ta.Rect = rc;
            r.PutText(ta);
        }

        public override void OnUpdate()
        {
            this.Thinking();
            this.Action();
        }

        #endregion

        public override bool Equals(object obj)
        {
            var p = obj as Player;
            if ((object)p == null)
                return false;
            return this.ID == p.ID;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public Player(PlayerInfo pi)
        {
            playerInfo = pi;
        }

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
                                factor.AddVector("RingLocation", team.TargetRingLocation.Location);
                                factor.AddVector("BallLocation", currentGame.Ball.Location.Location);
                                foreach (var p in team.CurrentEntries)
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
                        this.SetStateFactorDefence(factor);
                        break;
                    case TeamState.LooseBall:
                        factor.AddPrimitive("TeamState.LooseBall", true);
                        factor.AddVector("PlayerLocation", this.PlayerLocation.Location);
                        factor.AddVector("BallLocation", this.currentGame.Ball.Location.Location);
                        foreach (var p in team.CurrentEntries)
                            factor.AddVector("TeammateLocation", p.PlayerLocation.Location);
                        break;
                    case TeamState.StrategyTime:
                        factor.AddPrimitive("TeamState.StrategyTime", true);
                        break;
                }

                var s = this.AI.Determine(factor);
                currentState = s.State;
                targetLocation = CourtPos.FromVector(s.TargetLocation);
            }
        }

        private void SetStateFactorDefence(PropertyBag factor)
        {
            factor.AddPrimitive("TeamState.Defence", true);
            factor.AddVector("TargetLocation", team.GetDefaultPositionalLocation(this.CurrentPosition).Location);
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
                if (playerLocation.DistanceTo(this.CurrentGame.Ball.Location) < 5)
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
                if (this.CurrentGame.Ball.CurrentState != Ball.State.Passing)
                {
                    var playerDistance = playerLocation.DistanceTo(team.TargetRingLocation);
                    foreach (var player in team.CurrentEntries)
                    {
                        if (playerDistance > player.PlayerLocation.DistanceTo(team.TargetRingLocation))
                        {
                            hasBall = false;
                            this.CurrentGame.Ball.TargetLocation = player.PlayerLocation + CourtPos.FromVector(player.Direction) * (playerLocation.DistanceTo(player.PlayerLocation) / (float) 5);
                            this.CurrentGame.Ball.Force = 5;
                            this.CurrentGame.Ball.CurrentState = Ball.State.Passing;
                            currentState = PlayerState.Free;
                        }

                    }
                }
            }
            else if (currentState == PlayerState.Move)
            {
                this.Move(this.targetLocation);
            }
        }

        public void Move(CourtPos target)
        {
            var dir = target - playerLocation;
            this.diretion = dir.Location;
            dir.Location = dir.Location.Normalize();
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

        public UID ID
        {
            get { return playerInfo.ID; }
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
            get { return playerInfo.Image; }
        }

        public Team Team
        {
            get { return team; }
            set { team = value; }
        }

        public IPlayerAIType AI
        {
            get { return playerInfo.AI; }
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

        public string Name
        {
            get { return playerInfo.Name; }
        }

        public Game CurrentGame
        {
            get { return currentGame; }
            set { currentGame = value; }
        }

        public Position OriginalPosition
        {
            get { return playerInfo.Position; }
        }

        public Position CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }
    }
}

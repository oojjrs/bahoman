using System;
using System.Collections.Generic;

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
        private CourtPos direction;
        private CourtPos sight;
        private Game currentGame = null;
        private Position currentPosition = Position.Bench;
        private CourtPos targetLocation;

        #region From IDrawable

        public override void OnDraw(IRenderer r)
        {
            var loc = Court.ToGlobalLocation(this.Location);
            var la = new LineArgs(2);
            la.AddPoint(loc.Vector);
            la.AddPoint(loc.Vector + this.Sight.Vector * 30);
            r.PutLine(la);

            ImageArgs ia = new ImageArgs(playerInfo.Image);
            ia.Location = loc.Vector;
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
                        this.SetStateFactorAttack(factor);
                        break;
                    case TeamState.Defence:
                        this.SetStateFactorDefence(factor);
                        break;
                    case TeamState.LooseBall:
                        this.SetStateFactorLooseBall(factor);
                        break;
                    case TeamState.StrategyTime:
                        factor.AddValue("TeamState.StrategyTime", true);
                        break;
                }

                var s = this.AI.Determine(factor);
                currentState = s.State;
                targetLocation = s.TargetLocation;
            }
        }

        private void SetStateFactorAttack(PropertyBag factor)
        {
            factor.AddValue("TeamState.Attack", true);
            switch (currentState)
            {
                case PlayerState.Dribble:
                    factor.AddValue("PlayerState.Dribble", true);
                    factor.AddValue("TargetInfo.Type.Goal", true);
                    factor.AddValue("PlayerLocation", this.Location);
                    factor.AddValue("RingLocation", team.TargetRingLocation);
                    factor.AddValue("BallLocation", currentGame.Ball.Location);
                    foreach (var p in this.GetTeammates())
                        factor.AddValue("TeammateLocation", p.Location);
                    break;
                case PlayerState.Shoot:
                    factor.AddValue("PlayerState.Shoot", true);
                    break;
                case PlayerState.Pass:
                    factor.AddValue("PlayerState.Pass", true);
                    break;
                case PlayerState.Rebound:
                    factor.AddValue("PlayerState.Rebound", true);
                    break;
                case PlayerState.Free:
                    factor.AddValue("PlayerState.Free", true);
                    break;
                case PlayerState.FindBall:
                    factor.AddValue("PlayerState.FindBall", true);
                    break;
            }
        }

        private void SetStateFactorDefence(PropertyBag factor)
        {
            factor.AddValue("TeamState.Defence", true);
            factor.AddValue("TargetLocation", team.GetDefaultPositionalLocation(this.CurrentPosition));
        }

        private void SetStateFactorLooseBall(PropertyBag factor)
        {
            factor.AddValue("TeamState.LooseBall", true);
            factor.AddValue("PlayerLocation", this.Location);
            factor.AddValue("BallLocation", this.currentGame.Ball.Location);
            foreach (var p in this.GetTeammates())
                factor.AddValue("TeammateLocation", p.Location);
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
                    var t = this.GetPassableTarget();
                    if (t != null)
                    {
                        hasBall = false;
                        this.CurrentGame.Ball.TargetLocation = t.Location + t.Direction * (playerLocation.DistanceTo(t.Location) / (float)5);
                        this.CurrentGame.Ball.Force = 5;
                        this.CurrentGame.Ball.CurrentState = Ball.State.Passing;
                        currentState = PlayerState.Free;
                    }
                }
            }
            else if (currentState == PlayerState.Move)
            {
                this.Move(this.targetLocation);
            }
        }

        private Player GetPassableTarget()
        {
            Player ret = null;
            var distanceToRing = playerLocation.DistanceTo(team.TargetRingLocation);
            foreach (var t in this.GetTeammates())
            {
                var d = t.Location.DistanceTo(team.TargetRingLocation);
                if (d < distanceToRing)
                {
                    distanceToRing = d;
                    ret = t;
                }
            }
            return ret;
        }

        private List<Player> GetTeammates()
        {
            var entries = team.CurrentEntries;
            entries.Remove(this);
            var showentries = new List<Player>();

            foreach (var entry in entries)
            {
                var teammateDir = (entry.Location - this.Location).Normalize;
                var cosineTheta = ((this.Sight.X * teammateDir.X) + (this.Sight.Y * teammateDir.Y) + (this.Sight.Z * teammateDir.Z));
                var innerD = MyMath.RadianToDegree(Math.Acos(cosineTheta));
                if(innerD < 60)
                {
                    showentries.Add(entry);
                }
            }
            return showentries;
        }

        public void Move(CourtPos target)
        {
            var dir = target - playerLocation;
            if (dir.Length < 1.0f)
                return;

            direction = dir.Normalize;
            sight = dir.Normalize;
            playerLocation = playerLocation + direction;
            
            if (hasBall)
            {
                this.CurrentGame.Ball.CurrentState = Ball.State.Dribbling;
                this.CurrentGame.Ball.Location = this.CurrentGame.Ball.Location + direction;
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

        public UID ID
        {
            get { return playerInfo.ID; }
        }

        public CourtPos Location
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

        public CourtPos Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public CourtPos Sight
        {
            get { return sight; }
            set { sight = value; }
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

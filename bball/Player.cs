using System;
using System.Collections.Generic;
using System.Drawing;

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
        private PropertyBag factors;
        private int backNumber;

        public PlayerInfo(UID id)
        {
            this.id = id;
            name = "";
            birthday = new DateTime();
            image = null;
            ai = null;
            position = Position.Bench;
            backNumber = -1;
            factors = new PropertyBag();
        }

        public float GetFactor(string key)
        {
            float value = 0.0f;
            factors.GetValue(key, ref value);
            return value;
        }

        public void SetFactor(string key, float value)
        {
            if (value > 1.0f)
                value = 1.0f;
            if (value < 0.0f)
                value = 0.0f;
            if (factors.HasKey<float>(key))
                throw new Exception("귀찮아서 아직 처리 안함");
            factors.AddValue(key, value);
        }

        #region Properties
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

        public PropertyBag Factors
        {
            get { return factors; }
        }

        public int BackNumber
        {
            get { return backNumber; }
            set { backNumber = value; }
        }
        #endregion
    }

    class Player : Object, IMouseHandler
    {
        private readonly PlayerInfo playerInfo;
        private CourtPos playerLocation;
        private PlayerState currentState;
        private int lastThinkTick;
        private bool hasBall;
        private Team team;
        private float speed;
        private double playSightDegree;
        private CourtPos direction;
        private CourtPos sight;
        private Game currentGame;
        private Position currentPosition;
        private int elapsedTick;
        private AwarenessInfo awarenessInfo;
        private PlayerAIResult lastResult;

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
            var rc2 = new System.Drawing.Rectangle((int)loc.X, 0, 0, 0);
            using (var g = System.Drawing.Graphics.FromHwnd(r.GetHandle()))
            {
                var sizef = g.MeasureString(this.Name, OutputManager.DefaultFont);
                rc.Width = (int)sizef.Width + 1;
                rc.Height = (int)sizef.Height + 1;
                rc.Offset(-rc.Width / 2, (int)(rc.Height * 1.5));    // 위에서 이미지가 height/2 만큼 내려오므로 적당히 아래로 더 내린다.

                var sizef2 = g.MeasureString(currentState.ToString(), OutputManager.DefaultFont);
                rc2.Width = (int)sizef2.Width + 20;
                rc2.Height = (int)sizef2.Height + 1;
                rc2.Offset(-rc2.Width / 2, (int)rc2.Height + rc.Top);
            }

            var ta = TextArgs.Create(this.Name, OutputManager.DefaultFont);
            if (Log.Instance.TargetPlayer == this)
                ta.TextColor = new MyColor(Color.CadetBlue);

            ta.Format = TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            ta.Rect = rc;
            r.PutText(ta);

            ta.Text = currentState.ToString();
            ta.Rect = rc2;
            r.PutText(ta);
        }

        public override void OnUpdate()
        {
            Log.Instance.CurrentPlayer = this;

            var curTick = Environment.TickCount;
            if (curTick - lastThinkTick > 10 || lastThinkTick == 0)
            {
                var ret = this.Thinking();
                if (ret.UsePreviousResult == false)
                    lastResult = ret;
                lastThinkTick = curTick;
            }

            this.Action(lastResult);
            ++elapsedTick;

            Log.Instance.CurrentPlayer = null;
        }

        public virtual bool OnMouse(MouseArgs e)
        {
            Log.Instance.TargetPlayer = this;
            Log.Instance.ClearAITracing();
            return true;
        }

        public virtual Rectangle Zone
        {
            get
            {
                var rc = new Rectangle(Court.ToGlobalLocation(this.Location).ToPoint(), playerInfo.Image.Size);
                rc.Offset(-playerInfo.Image.Size.Width / 2, -playerInfo.Image.Size.Height / 2);
                return rc;
            }
        }

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

            InputManager.Add(this);
        }

        public void Clear()
        {
            playerLocation = CourtPos.Center;
            currentState = PlayerState.Ready;
            lastThinkTick = 0;
            hasBall = false;
            speed = 1;
            playSightDegree = 120;
            direction = CourtPos.Center;
            sight = CourtPos.Center;
            currentGame = null;
            currentPosition = Position.Bench;
            elapsedTick = 0;
            awarenessInfo = new AwarenessInfo();
            lastResult = new PlayerAIResult();
        }

        private PlayerAIResult Thinking()
        {
            var factor = new PropertyBag();
            playerInfo.Factors.CopyTo(factor);

            factor.AddValue("TeamState." + team.TeamState.ToString(), true);
            factor.AddValue("PlayerState." + currentState.ToString(), true);
            factor.AddValue("PlayerLocation", this.Location);
            factor.AddValue("RingLocation", team.TargetRingLocation);
            factor.AddValue("BallLocation", this.currentGame.Ball.Location);
            factor.AddValue("HomePositionLocation", team.GetDefaultPositionalLocation(this.CurrentPosition));
            factor.AddValue("AwayPositionLocation", team.Away.GetDefaultPositionalLocation(this.CurrentPosition));
            factor.AddValue("AwarenessInfo", this.awarenessInfo);
            factor.AddValue("Direction", this.Direction);

            if (IsShow(this.Location, this.currentGame.Ball.Location))
            {
                factor.AddValue("Ball", true);
                factor.AddValue("BallLocation", this.currentGame.Ball.Location);
                factor.AddValue("BallState", this.currentGame.Ball.CurrentState);
                factor.AddValue("IsThrower", currentGame.Ball.Thrower == this);
            }

            return this.AI.Determine(factor);
        }

        private void Action(PlayerAIResult ret)
        {
            direction = (ret.TargetLocation - playerLocation).Normalize;
            this.SetState(ret);

            switch (currentState)
            {
                case PlayerState.Dribble:
                    this.DoMove(ret);
                    break;
                case PlayerState.Shoot:
                    break;
                case PlayerState.OffBall:
                    this.DoMove(ret);
                    break;
                case PlayerState.FindBall:
                    this.DoMove(ret);
                    break;
                case PlayerState.Move:
                    this.DoMove(ret);
                    break;
            }
        }

        private bool IsShow(CourtPos began, CourtPos target)
        {
            var targetDir = (target - began).Normalize;
            var cosineTheta = ((this.Sight.X * targetDir.X) + (this.Sight.Y * targetDir.Y) + (this.Sight.Z * targetDir.Z));
            var innerD = MyMath.RadianToDegree(Math.Acos(cosineTheta));

            if (innerD < playSightDegree)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void Seeing()
        {
            var teamEntries = team.CurrentEntries;
            teamEntries.Remove(this);
            foreach (var entry in teamEntries)
            {
                if (IsShow(this.Location, entry.Location))
                {
                    awarenessInfo.PlayerAwarenessInfos.Add(new PlayerAwareness(entry.Location, entry.currentState, true, entry.playerInfo.BackNumber, entry.hasBall, Environment.TickCount, entry.Direction));
                }
            }
           
            var awayTeamEntries = team.Away.CurrentEntries;
            foreach (var entry in awayTeamEntries)
            {
                if (IsShow(this.Location, entry.Location))
                {
                    awarenessInfo.PlayerAwarenessInfos.Add(new PlayerAwareness(entry.Location, entry.currentState, false, entry.playerInfo.BackNumber, entry.hasBall, Environment.TickCount, entry.Direction));
                }
            }

            if (IsShow(this.Location, currentGame.Ball.Location))
            {
                awarenessInfo.BallInfo.BallState = currentGame.Ball.CurrentState;
                awarenessInfo.BallInfo.Location = currentGame.Ball.Location;
                awarenessInfo.BallInfo.Direction = currentGame.Ball.Direction;
                awarenessInfo.BallInfo.Velocity = currentGame.Ball.Force;
                awarenessInfo.BallInfo.LastAwarenessTick = Environment.TickCount;
            }
        }

        public void DoMove(PlayerAIResult ret)
        {
            if ((int)(elapsedTick * playerInfo.GetFactor("Sight")) % 10 == 0)
            {
                sight = (currentGame.Ball.Location - playerLocation).Normalize;
                Seeing();
            }

            if ((ret.TargetLocation - playerLocation).Length >= 1.0f)
                playerLocation = playerLocation + direction;
            
            if (hasBall)
            {
                this.CurrentGame.Ball.CurrentState = BallState.Dribbling;
                this.CurrentGame.Ball.Location = this.CurrentGame.Ball.Location + direction;
            }
        }

        public void SetState(PlayerAIResult ret)
        {
            var nextState = ret.State;
            if (nextState != currentState)
            {
                this.EndState(ret, currentState);
                nextState = this.BeginState(ret);
            }

            // Note : 일단 병신 구조지만 나중에 슛을 구현할 때 같이 일반화하도록 한다.
            currentState = nextState;
        }

        public PlayerState BeginState(PlayerAIResult ret)
        {
            switch(ret.State)
            {
                case PlayerState.Ready:
                    break;
                case PlayerState.CatchBall:
                    hasBall = true;
                    this.CurrentGame.SetTeamState(team, TeamState.Attack);
                    break;
                case PlayerState.Dribble:
                    sight = direction;
                    this.CurrentGame.SetTeamState(team, TeamState.Attack);
                    this.CurrentGame.Ball.CurrentState = BallState.Dribbling;
                    break;
                case PlayerState.FindBall:
                    sight = direction;
                    break;
                case PlayerState.OffBall:
                    sight = direction;
                    break;
                case PlayerState.Move:
                    sight = direction;
                    break;
                case PlayerState.Pass:
                    hasBall = false;
                    this.CurrentGame.Ball.Direction = ret.BallDirection;
                    this.CurrentGame.Ball.Force = ret.BallVelocity;
                    this.CurrentGame.Ball.Thrower = this;
                    this.CurrentGame.Ball.CurrentState = BallState.Passing;
                    this.CurrentGame.SetTeamState(team, TeamState.LooseBall);
                    return PlayerState.OffBall;
                case PlayerState.Rebound:
                    break;
                case PlayerState.Shoot:
                    this.CurrentGame.Ball.TargetLocation = ret.TargetLocation;
                    this.CurrentGame.Ball.CurrentState = BallState.Shooting;
                    break;
                case PlayerState.Stand:
                    break;
                default:
                    throw new Exception("추가된 PlayerState에 대한 처리가 필요합니다.");
            }
            return ret.State;
        }

        public void EndState(PlayerAIResult ret, PlayerState oldState)
        {
            switch (oldState)
            {
                case PlayerState.Ready:
                    break;
                case PlayerState.CatchBall:
                    break;
                case PlayerState.Dribble:
                    break;
                case PlayerState.FindBall:
                    break;
                case PlayerState.OffBall:
                    break;
                case PlayerState.Move:
                    break;
                case PlayerState.Pass:
                    break;
                case PlayerState.Rebound:
                    break;
                case PlayerState.Shoot:
                    break;
                case PlayerState.Stand:
                    break;
                default:
                    throw new Exception("추가된 PlayerState에 대한 처리가 필요합니다.");
            }
        }

        public void SetFactor(string key, float value)
        {
            playerInfo.SetFactor(key, value);
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

        public bool HasBall
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

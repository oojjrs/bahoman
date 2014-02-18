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

    class Player : Object
    {
        private readonly PlayerInfo playerInfo;
        private CourtPos playerLocation;
        private PlayerState currentState;
        private int lastThinkTick;
        private Boolean hasBall = false;
        private Team team = null;
        private float speed = 1;
        private double playSightDegree = 120;
        private CourtPos direction;
        private CourtPos sight;
        private Game currentGame = null;
        private Position currentPosition = Position.Bench;
        private CourtPos targetLocation;
        private int elapsedTick = 0;
        private AwarenessInfo awarenessInfo = null;

        // Note : 아래는 임시 변수들
        private CourtPos ballDirection;
        private float ballVelocity;

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
            ta.Format = TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            ta.Rect = rc;
            r.PutText(ta);

            ta.Text = currentState.ToString();
            ta.Rect = rc2;
            r.PutText(ta);
        }

        public override void OnUpdate()
        {
            var curTick = Environment.TickCount;
            if (curTick - lastThinkTick > 10 || lastThinkTick == 0)
            {
                var ret = this.Thinking();
                if (ret.UsePreviousResult == false)
                {
                    ballDirection = ret.BallDirection;
                    ballVelocity = ret.BallVelocity;

                    this.SetTargetLocation(ret.TargetLocation);
                    this.SetState(ret.State);
                }
                lastThinkTick = curTick;
            }

            this.Action();
            ++elapsedTick;
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
            targetLocation = CourtPos.Center;
            elapsedTick = 0;
            awarenessInfo = new AwarenessInfo();
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

        private void Action()
        {
            switch(currentState)
            {
                case PlayerState.Dribble:
                    this.Move(targetLocation);
                    break;
                case PlayerState.Shoot:
                    break;
                case PlayerState.Free:
                    this.Move(targetLocation);
                    break;
                case PlayerState.FindBall:
                    this.DoFindBall();
                    break;
                case PlayerState.Pass:
                    this.DoPass();
                    break;
                case PlayerState.CatchBall:
                    this.DoCatchBall();
                    break;
                case PlayerState.Move:
                    this.Move(targetLocation);
                    break;
            }
        }

        private void DoFindBall()
        {
            if (playerLocation.DistanceTo(this.CurrentGame.Ball.Location) < 5)
            {
                hasBall = true;
                SetState(PlayerState.Dribble);
            }
            else
            {
                this.Move(targetLocation);
            }
        }

        private void DoPass()
        {
        }

        private void DoCatchBall()
        {
            if (hasBall)
            {
                SetState(PlayerState.Dribble);
            }
            else
            {
                if (playerLocation.DistanceTo(this.CurrentGame.Ball.Location) < 5)
                {
                    hasBall = true;
                    SetState(PlayerState.Dribble);
                }
                else
                {
                    this.Move(targetLocation);
                }
            }
        }

        private Boolean IsShow(CourtPos began, CourtPos target)
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

        public void Move(CourtPos target)
        {
            if ((int)(elapsedTick * playerInfo.GetFactor("Sight")) % 10 == 0)
            {
                sight.RotateY((float)MyMath.DegreeToRadian(60.0));
                sight = sight.Normalize;
                Seeing();
            }

            if ((target - playerLocation).Length >= 1.0f)
                playerLocation = playerLocation + direction;
            
            if (hasBall)
            {
                this.CurrentGame.Ball.CurrentState = BallState.Dribbling;
                this.CurrentGame.Ball.Location = this.CurrentGame.Ball.Location + direction;
            }
        }

        private void SetTargetLocation(CourtPos target)
        {
            targetLocation = target;
            direction = (target - playerLocation).Normalize;
        }

        public void SetState(PlayerState playerstate)
        {
            if (playerstate != currentState)
            {
                this.EndState(currentState);
                this.BeginState(playerstate);
            }

            currentState = playerstate;
        }

        public void BeginState(PlayerState state)
        {
            switch(state)
            {
                case PlayerState.Ready:
                    break;
                case PlayerState.CatchBall:
                    break;
                case PlayerState.Dribble:
                    sight = direction;
                    this.CurrentGame.SetTeamState(team, TeamState.Attack);
                    this.CurrentGame.Ball.CurrentState = BallState.Dribbling;
                    break;
                case PlayerState.FindBall:
                    sight = direction;
                    break;
                case PlayerState.Free:
                    sight = direction;
                    break;
                case PlayerState.Move:
                    sight = direction;
                    break;
                case PlayerState.Pass:
                    if (this.CurrentGame.Ball.CurrentState != BallState.Passing)
                    {
                        hasBall = false;
                        this.CurrentGame.Ball.Direction = ballDirection;
                        this.CurrentGame.Ball.Force = ballVelocity;
                        this.CurrentGame.Ball.Thrower = this;
                        this.CurrentGame.Ball.CurrentState = BallState.Passing;
                        currentState = PlayerState.Free;
                    }
                    break;
                case PlayerState.Rebound:
                    break;
                case PlayerState.Shoot:
                    this.CurrentGame.Ball.TargetLocation = targetLocation;
                    this.CurrentGame.Ball.CurrentState = BallState.Shooting;
                    break;
                case PlayerState.Stand:
                    break;
                default:
                    throw new Exception("추가된 PlayerState에 대한 처리가 필요합니다.");
            }
        }

        public void EndState(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Ready:
                    break;
                case PlayerState.CatchBall:
                    break;
                case PlayerState.Dribble:
                    break;
                case PlayerState.FindBall:
                    break;
                case PlayerState.Free:
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

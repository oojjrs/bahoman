using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public class PlayerAwareness
    {
        private CourtPos playerLocation;
        private PlayerState playerState;
        private Boolean isTeammate;
        private int backNumber;
        private Boolean hasBall;
        private int lastAwarenessTick;
        private CourtPos direction;

        public PlayerAwareness(CourtPos location, PlayerState state, Boolean isteammate, int backnumber ,Boolean hasball, int lastawarenesstick, CourtPos dir)
        {
            playerLocation = location;
            playerState = state;
            isTeammate = isteammate;
            hasBall = hasball;
            backNumber = backnumber;
            lastAwarenessTick = lastawarenesstick;
            direction = dir;
        }

        public PlayerState PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

        public CourtPos Location
        {
            get { return playerLocation; }
            set { playerLocation = value; }
        }

        public Boolean HasBall
        {
            get { return hasBall; }
            set { hasBall = value; }
        }

        public Boolean IsTeammate
        {
            get { return isTeammate; }
            set { isTeammate = value; }
        }

        public int LastAwarenessTick
        {
            get { return lastAwarenessTick; }
            set { lastAwarenessTick = value; }
        }

        public CourtPos Direction
        {
            get { return direction; }
        }
    }

    public class BallAwareness
    {
        private BallState ballState;
        private CourtPos ballLocation;
        private int lastAwarenessTick = 0;

        public BallAwareness()
        {
        }

        public BallState BallState
        {
            get { return ballState; }
            set { ballState = value; }
        }

        public CourtPos Location
        {
            get { return ballLocation; }
            set { ballLocation = value; }
        }

        public int LastAwarenessTick
        {
            get { return lastAwarenessTick; }
            set { lastAwarenessTick = value; }
        }
    }
    
    public class AwarenessInfo
    {
        private List<PlayerAwareness> playerAwarenessInfos;
        private BallAwareness ballInfo;

        public AwarenessInfo()
        {
            playerAwarenessInfos = new List<PlayerAwareness>();
            ballInfo = new BallAwareness();
        }

        public List<PlayerAwareness> PlayerAwarenessInfos
        {
            get { return playerAwarenessInfos; }
            set { playerAwarenessInfos = value; }
        }

        public BallAwareness BallInfo
        {
            get { return ballInfo; }
            set { ballInfo = value; }
        }
    }
}

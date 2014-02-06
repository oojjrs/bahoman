using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    class PlayerAwareness
    {

        private CourtPos playerLocation;
        private PlayerState playerState;
        private Boolean isTeammate;
        private Boolean hasBall;
        private int lastAwarenessTick = 0;

        public PlayerAwareness(CourtPos location, PlayerState state, Boolean isteammate, Boolean hasball, int lastawarenesstick)
        {
            playerLocation = location;
            playerState = state;
            isTeammate = isteammate;
            hasBall = hasball;
            lastAwarenessTick = lastawarenesstick;
        }

        public PlayerState PlayerState
        {
            get { return playerState; }
        }

        public CourtPos Location
        {
            get { return playerLocation; }
        }

        public Boolean HasBall
        {
            get { return hasBall; }
        }

        public Boolean IsTeammate
        {
            get { return isTeammate; }
        }

        public int LastAwarenessTick
        {
            get { return lastAwarenessTick; }
        }
    }

    class BallAwareness
    {
        private BallState ballState;
        private CourtPos ballLocation;
        private int lastAwarenessTick = 0;

        public BallAwareness(BallState state, CourtPos location, int lastawarenesstick)
        {
            ballLocation = location;
            ballState = state;
            lastAwarenessTick = lastawarenesstick;
        }

        public BallState BallState
        {
            get { return ballState; }
        }

        public CourtPos Location
        {
            get { return ballLocation; }
        }

        public int LastAwarenessTick
        {
            get { return lastAwarenessTick; }
        }
    }
    
    class AwarenessInfo
    {
        
    }
}

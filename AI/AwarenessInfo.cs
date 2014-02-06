﻿using System;
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
        private int backNumber;
        private Boolean hasBall;
        private int lastAwarenessTick;

        public PlayerAwareness(CourtPos location, PlayerState state, Boolean isteammate, int backnumber ,Boolean hasball, int lastawarenesstick)
        {
            playerLocation = location;
            playerState = state;
            isTeammate = isteammate;
            hasBall = hasball;
            backNumber = backnumber;
            lastAwarenessTick = lastawarenesstick;
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
    
    class AwarenessInfo
    {
        private List<PlayerAwareness> playerAwarenessInfos = new List<PlayerAwareness>();
        private BallAwareness ballInfo = null;

        public AwarenessInfo()
        { 
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
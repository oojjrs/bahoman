﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AI;

namespace bball
{
    class Game
    {
        private Court court = new Court();
        private Team homeTeam = null;
        private Team awayTeam = null;

        public bool Initialize(Team home, Team away)
        {
            court.Visible = true;
            court.Ball.Visible = true;  // 볼은 어디에 있어야 맞는 걸까?

            homeTeam = home;
            if (homeTeam.Initialize(this, away, BaseCourt.Left) == false)
                return false;

            awayTeam = away;
            if (awayTeam.Initialize(this, home, BaseCourt.Right) == false)
                return false;
            return true;
        }

        public void SetTeamState(Team target, TeamState state)
        {
            target.TeamState = TeamState.Attack;
            target.Away.TeamState = TeamState.Defence;
        }

        public Court Court
        {
            get { return court; }
        }

        public Ball Ball
        {
            get { return court.Ball; }
        }
    }
}

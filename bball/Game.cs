using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using AI;
using Renderer;

namespace bball
{
    class Game : Object
    {
        private Court court = new Court();
        private Team homeTeam = null;
        private Team awayTeam = null;

        public override void OnDraw(IRenderer r)
        {
            var msg = String.Format("HomeState : {0}\r\nAwayState : {1}", homeTeam.TeamState, awayTeam.TeamState);
            var ht = TextArgs.Create(msg, OutputManager.DefaultFont);
            ht.Rect = new Rectangle(810, 10, 400, 150);
            r.PutText(ht);
        }

        public override void OnUpdate()
        {
        }

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

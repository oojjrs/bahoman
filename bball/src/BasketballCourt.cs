using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bball.src
{
    class BasketballCourt
    {
        private int homeScore;
        private int awayScore;

        public int AddHomeScore(int point)
        {
            homeScore = homeScore + point;
            return homeScore;
        }

        public int AddAwayScore(int point)
        {
            awayScore = awayScore + point;
            return awayScore;
        }

        public int HomeScore
        {
            get { return this.homeScore; }
        }

        public int AwayScore
        {
            get { return this.awayScore; }
        }
    }
}

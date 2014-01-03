namespace AI
{
    class BasketballCourt
    {
        private int homescore;
        private int awayscore;

        public int AddHomeScore(int point)
        {
            homescore = homescore + point;
            return homescore;
        }

        public int AddAwayScore(int point)
        {
            awayscore = awayscore + point;
            return awayscore;
        }

        public int HomeScore
        {
            get { return this.homescore; }
        }

        public int awayScore
        {
            get { return this.homescore; }
        }

    }
}

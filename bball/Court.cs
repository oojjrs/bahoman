using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using AI;
using Core;
using Physics;
using Renderer;

namespace bball
{
    sealed class Court : Object
    {
        #region Variables
        private static Court instance = null;
        private IImage image = null;
        private int homeScore;
        private int awayScore;
        private Ball ball = new Ball();

        #endregion

        #region From IDrawable

        public override void OnDraw(IRenderer r)
        {
            ImageArgs ia = new ImageArgs(image);
            r.PutImage(ia);
        }

        public override void OnUpdate()
        {
        }

        #endregion

        #region Methods
        private Court()
        {
        }

        public static Court Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new Court();
                }
                return instance;
            }
        }

        public static CourtPos ToGlobalLocation(CourtPos pos)
        {
            var np = new CourtPos();
            np.X += Court.Width / 2;
            np.Y = pos.Y;
            np.Z += Court.Height / 2;
            return np;
        }

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

        public void CreateBall(IImage ballimage)
        {
            ball.Image = ballimage;
            ball.Location = CourtPos.FromCoord(-150, 0, 0);
        }

        #endregion

        #region Properties

        public static int Width
        {
            get { return 1290; }
        }

        public static int Height
        {
            get { return 968; }
        }

        public static CourtPos LeftGoalPos
        {
            get { return CourtPos.FromCoord(-550, 0, 0); }
        }

        public static CourtPos RightGoalPos
        {
            get { return CourtPos.FromCoord(550, 0, 0); }
        }

        public int HomeScore
        {
            get { return this.homeScore; }
        }

        public int AwayScore
        {
            get { return this.awayScore; }
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        #endregion
    }
}

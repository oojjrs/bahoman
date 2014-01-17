using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

        public void CreateBall(IImage ballimage)
        {
            ball.Image = ballimage;
            ball.Location = new Point(-150, 0);
        }

        public static Point LogicalCoordToPhysicalCoord(int x, int y)
        {
            return Court.LogicalCoordToPhysicalCoord(new Point(x, y));
        }

        public static Point LogicalCoordToPhysicalCoord(Point pt)
        {
            pt.X = pt.X + Court.Width / 2;
            pt.Y = pt.Y + Court.Height / 2;
            return pt;
        }

        public static Vector3f LogicalCoordToPhysicalCoord(Vector3f pos)
        {
            var np = new Vector3f(pos.X, pos.Y, pos.Z);
            np.X += Court.Width / 2;
            np.Z += Court.Width / 2;
            return pos;
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

        public static Point LeftGoalPos
        {
            get { return new Point(-550, 0); }
        }

        public static Point RightGoalPos
        {
            get { return new Point(550, 0); }
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

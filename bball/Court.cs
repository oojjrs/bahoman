using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;
using Renderer;

namespace bball
{
    class Court : Object
    {
        #region Variables

        private IImage image = null;
        private int homeScore;
        private int awayScore;

        #endregion

        #region From IDrawable

        public override void OnDraw(IRenderer r)
        {
            ImageArgs ia = new ImageArgs(image);
            r.PutImage(ia);
        }

        #endregion

        #region Methods

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

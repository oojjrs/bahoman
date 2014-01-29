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
        private IImage image = ImageFactory.Create("res/court.png");
        private Ball ball = new Ball();
        private static Random random = new Random();

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
        public static CourtPos ToGlobalLocation(CourtPos pos)
        {
            var np = new CourtPos();
            np.X = pos.X + Court.ImageWidth / 2;
            np.Y = pos.Y;
            np.Z = pos.Z + Court.ImageHeight / 2;
            return np;
        }

        public static CourtPos ToLogicalLocation(Vector3f v)
        {
            var np = new CourtPos();
            np.X = v.X - Court.ImageWidth / 2;
            np.Y = v.Y;
            np.Z = v.Z - Court.ImageHeight / 2;
            return np;
        }

        public static CourtPos CreateRandomPos()
        {
            var v = new Vector3f(random.Next(Court.Width) - Court.Width / 2, 0, random.Next(Court.Height) - Court.Height / 2);
            return CourtPos.FromVector(v);
        }

        public Court()
        {
            OutputManager.MoveToFirst(this);
        }

        #endregion

        #region Properties

        public static int ImageWidth
        {
            get { return 1290; }
        }

        public static int ImageHeight
        {
            get { return 968; }
        }

        public static int Width
        {
            get { return 1243; }
        }

        public static int Height
        {
            get { return 665; }
        }

        public static CourtPos LeftGoalPos
        {
            get { return CourtPos.FromCoord(-550, 0, 0); }
        }

        public static CourtPos RightGoalPos
        {
            get { return CourtPos.FromCoord(550, 0, 0); }
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        public Ball Ball
        {
            get { return ball; }
        }
        #endregion
    }
}

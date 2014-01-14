using System;
using System.Drawing;

using Core;
using Renderer;
using AI;
using System.Windows.Forms;

namespace bball
{
    class Player : Object
    {
        private Point playerPosition = new Point();
        private PlayerState currentState = new PlayerState();
        //private Team team = new Team();
        //private PlayerState prevState = new PlayerState();
        private IImage image = null;

        #region From IDrawable
        public override void OnDraw(IRenderer r)
        {
            var pt = Court.LogicalCoordToPhysicalCoord(playerPosition);
            ImageArgs ia = new ImageArgs(image);
            ia.SetPos(pt.X, pt.Y);
            ia.CorrectToCenter = true;
            r.PutImage(ia);
        }
        #endregion

        public Player(int x, int y, Team team, IRenderer r)
        {
            playerPosition = new Point(x, y);
            //teamType = teamtype;
            this.SetImage(r.GetImage("res/Player.png", new MyColor(), "Player"));
        }

        public void SetImage(IImage image)
        {
            this.image = image;
        }

        public void Thinking()
        {
            var factor = new DetermineFactor();
            factor.CurrentState = currentState;
            factor.PlayerPosition = playerPosition;

            var targetInfo = new TargetInfo();
            targetInfo.Position = new Point(550, 0);
            targetInfo.TargetType = TargetInfo.Type.Goal;
            factor.TargetInfo = targetInfo;

            currentState = PlayerAI.Determine(factor);
        }

        public void Action()
        {
            if (currentState == PlayerState.Dribble)
            {
                playerPosition = new Point(playerPosition.X + 1, playerPosition.Y);
            }
            else if (currentState == PlayerState.Shoot)
            {
                MessageBox.Show("Ω∏¿Ã¥Ÿ");
            }
        }

        public Point PlayerPosition
        {
            get
            {
                return playerPosition;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;
using Renderer;
using AI;
using System.Windows.Forms;

namespace bball
{
    class Player : Object
    {
        private Point playerPosition = new Point(0,0);
        private PlayerState currentState = new PlayerState();
        private TeamType teamType = new TeamType();
        //private PlayerState nextState = new PlayerState();
        private IImage image = null;

        #region From IDrawable
        public override void OnDraw(IRenderer r)
        {
            var pt = Court.GetCoordinate(playerPosition);
            ImageArgs ia = new ImageArgs(image);
            ia.SetPos(pt.X, pt.Y);
            ia.CorrectToCenter = true;
            r.PutImage(ia);
        }
        #endregion

        public Player(int x, int y, TeamType teamtype, IRenderer r)
        {
            playerPosition = new Point(x, y);
            teamType = teamtype;
            this.SetImage(r.GetImage("res/Player.png", new MyColor(), "Player"));
        }

        public void SetImage(IImage image)
        {
            this.image = image;
        }

        public void Thinking()
        {
            currentState = PlayerAI.Determine(currentState, playerPosition);
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
                //return GetPlayerPotion(playerposition);
                //var pt = rc.Location;
                //pt.Offset(15, 15);
                //return pt;
            }
        }
    }
}

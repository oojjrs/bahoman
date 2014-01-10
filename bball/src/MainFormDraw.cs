using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core;
using Renderer;

namespace bball
{
    public partial class MainForm : Form
    {
        private void OnDraw(IRenderer r)
        {
            ImageArgs ia = new ImageArgs();
            ia.image = renderer.GetImage("res/court.png", new MyColor(), "court");
            //ia.sx = 0.7f;
            //ia.sy = 0.7f;
            r.PutImage(ia);
        }
    }
}

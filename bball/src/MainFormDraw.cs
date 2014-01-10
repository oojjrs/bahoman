using System;
using System.Collections.Generic;
using System.Drawing;
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
            ia.image = r.GetImage("res/court.png", new MyColor(), "court");
            r.PutImage(ia);

            pm.OnDraw(r);
        }
    }
}

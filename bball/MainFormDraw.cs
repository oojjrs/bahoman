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
            OutputManager.OutputAll(r);

            var ta = TextArgs.Create("F4 : 로그창 열기, F5 : 재시작, F9 : 업데이트 정지", OutputManager.DefaultFont);
            ta.Rect = new Rectangle(10, 10, 500, 50);
            r.PutText(ta);
        }
    }
}

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

            var msg = String.Format("Tick : {0}\r\nF4   : 로그창 열기\r\nF5   : 재시작\r\nF9   : 업데이트 정지\r\nF10  : (정지 상태에서) 한 프레임씩 재생", totalTickCount);
            var ta = TextArgs.Create(msg, OutputManager.DefaultFont);
            ta.Rect = new Rectangle(10, 10, 500, 150);
            r.PutText(ta);

            // Note : 귀찮아서 ^^;
            game.OnDraw(r);
        }
    }
}

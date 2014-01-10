using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct3D9;

using Core;

namespace Renderer
{
    public interface IRenderer
    {
	    bool BeginClip(System.Drawing.Rectangle Viewport);	// viewport 영역만 보이고 나머지가 클리핑된다
	    bool BeginDraw();
	    bool Clear(MyColor color);
	    bool EndClip();
	    bool EndDraw();
	    bool Flip();
	    bool Flip(int x, int y);
	    bool Flip(int x, int y, int cx, int cy);
        IImage GetImage(string path, MyColor key, string identifier);
	    bool Initialize(Form mainWindow);
	    bool PutImage(ImageArgs r);
	    bool PutLine(LineArgs r);
        bool PutRect(MyColor cr);
        bool PutRect(int left, int top, int right, int bottom, MyColor cr);
        bool PutText(TextArgs r);
	    bool ResizeBackBuffer(int width, int height);
	    void SetReporter(IReporter er);
	    void SetVisibleFrameRate(bool bSet);	// out to window caption

        string Identifier { get; }
    }
}

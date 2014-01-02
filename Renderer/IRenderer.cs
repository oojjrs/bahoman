using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Renderer
{
    interface IRenderer
    {
	    bool BeginClip(Rectangle Viewport);	// viewport 영역만 보이고 나머지가 클리핑된다
	    bool BeginDraw();
	    bool Clear(Color color);
	    bool EndClip();
	    bool EndDraw();
	    bool Flip();
	    bool Flip(int x, int y);
	    bool Flip(int x, int y, int cx, int cy);
	    //IFont GetFont(Font font);
	    string GetIdentifier();
        //IImage GetImage(string path, Color key, string identifier);
        //IImage GetImage(int width, int height, Color fill, string identifier);
	    bool Initialize(IntPtr hMainWindow);
	    bool PutRect(Color cr);
	    bool PutRect(int left, int top, int right, int bottom, Color cr);
	    bool PutImage(ImageArgs r);
	    bool PutLine(LineArgs r);
	    bool PutText(TextArgs r);
	    bool ResizeBackBuffer(int width, int height);
	    void SetReporter(IReporter er);
	    void SetVisibleFrameRate(bool bSet);	// out to window caption
    }
}

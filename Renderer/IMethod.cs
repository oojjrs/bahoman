using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

using Core;

namespace Renderer
{
    public interface IMethod
    {
        bool OnBeginClip(Rectangle r);
        bool OnBeginDraw();
        bool OnClear(Color color);
        bool OnEndClip();
        bool OnEndDraw();
        bool OnFlip(Rectangle rcDest);
        //IFont* OnGetFont(string name, int size, string identifier);
        //IFont* OnGetFont(const LOGFONTW& r, string identifier);
        string OnGetIdentifier();
        //IImage* OnGetImage(string path, Color key, string identifier);
        //IImage* OnGetImage(int width, int height, Color fill, string identifier);
        bool OnInitialize(Form mainWindow);
        bool OnPutImage(ImageArgs r);
        bool OnPutLine(LineArgs r);
        bool OnPutRect(Color cr);
        bool OnPutRect(Rectangle rc, Color cr);
        bool OnPutText(TextArgs r);
        bool OnResizeBackBuffer(int width, int height);
        bool OnRestoreDevice(int width, int height);

        void SetReporter(IReporter r);
    }
}

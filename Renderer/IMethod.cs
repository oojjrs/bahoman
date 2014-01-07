﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct3D9;

using Core;

namespace Renderer
{
    public interface IMethod
    {
        bool OnBeginClip(System.Drawing.Rectangle r);
        bool OnBeginDraw();
        bool OnClear(MyColor color);
        bool OnEndClip();
        bool OnEndDraw();
        bool OnFlip(System.Drawing.Rectangle rcDest);
        //IFont* OnGetFont(string name, int size, string identifier);
        //IFont* OnGetFont(const LOGFONTW& r, string identifier);
        string OnGetIdentifier();
        //IImage* OnGetImage(string path, Color key, string identifier);
        //IImage* OnGetImage(int width, int height, Color fill, string identifier);
        bool OnInitialize(Form mainWindow);
        bool OnPutImage(ImageArgs r);
        bool OnPutLine(LineArgs r);
        bool OnPutRect(MyColor cr);
        bool OnPutRect(System.Drawing.Rectangle rc, MyColor cr);
        bool OnPutText(TextArgs r);
        bool OnResizeBackBuffer(int width, int height);
        bool OnRestoreDevice(int width, int height);

        void SetReporter(IReporter r);
    }
}

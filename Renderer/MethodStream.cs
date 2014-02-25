using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;

namespace Renderer
{
    class StreamImage : IImage
    {
        string identifier;

        public string GetIdentifier()
        {
            return identifier;
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(); }
        }

        public StreamImage(string id)
        {
            identifier = id;
        }
    }

    class MethodStream : IMethod
    {
        LogHelper log = new LogHelper();

        #region From IMethod
        public bool OnBeginClip(System.Drawing.Rectangle r)
        {
            log.Log("OnBeginClip({0})", r);
            return true;
        }

        public bool OnBeginDraw()
        {
            log.Log("OnBeginDraw()");
            return true;
        }

        public bool OnClear(MyColor color)
        {
            log.Log("OnClear({0})", color);
            return true;
        }

        public bool OnEndClip()
        {
            log.Log("OnEndClip()");
            return true;
        }

        public bool OnEndDraw()
        {
            log.Log("OnEndDraw()");
            return true;
        }

        public bool OnFlip(System.Drawing.Rectangle rcDest)
        {
            log.Log("OnFlip({0})", rcDest);
            return true;
        }

        public string OnGetIdentifier()
        {
            return "Renderer via Stream";
        }

        public IImage OnGetImage(string path, MyColor key, string identifier)
        {
            log.Log("OnGetImage({0}, {1}, {2})", path, key, identifier);
            return new StreamImage(identifier);
        }

        public bool OnInitialize(System.Windows.Forms.Form mainWindow)
        {
            log.Log("OnInitialize({0})", mainWindow);
            return true;
        }

        public bool OnPutImage(ImageArgs r)
        {
            log.Log("OnPutImage({0})", r);
            return true;
        }

        public bool OnPutLine(LineArgs r)
        {
            log.Log("OnPutLine({0})", r);
            return true;
        }

        public bool OnPutRect(MyColor cr)
        {
            log.Log("OnPutRect({0})", cr);
            return true;
        }

        public bool OnPutRect(System.Drawing.Rectangle rc, MyColor cr)
        {
            log.Log("OnPutRect({0}, {1})", rc, cr);
            return true;
        }

        public bool OnPutText(TextArgs r)
        {
            log.Log("OnPutText({0})", r);
            return true;
        }

        public bool OnResizeBackBuffer(int width, int height)
        {
            log.Log("OnResizeBackBuffer({0}, {1})", width, height);
            return true;
        }

        public bool OnRestoreDevice(int width, int height)
        {
            log.Log("OnRestoreDevice({0}, {1})", width, height);
            return true;
        }

        public void SetReporter(IReporter r)
        {
            log.SetReporter(r);
        }
        #endregion
    }
}

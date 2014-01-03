using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;

namespace Renderer
{
    class Engine : IRenderer
    {
#region Variables
        bool isInit = false;             // 초기화되었는가
        bool isLostDevice = false;
        bool isReadyBuffer = false;      // 그리기 준비가 되었다(BeginDraw에서 활성화, EndDraw에서 비활성화)
        bool isVisibleFrameRate = false;
        int backBufferHeight = 0;
        int backBufferWidth = 0;
        IntPtr mainWindowHandle = IntPtr.Zero;
        readonly IMethod api = null;
        //int tickCount = 0;
        LogHelper log = new LogHelper();
#endregion

#region From IRenderer
        public bool BeginClip(Rectangle viewport)
        {
            if (isInit == false)
            {
                log.Assert("BeginClip({0}) : NOT INITIALIZED", viewport.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("BeginClip({0}) : NOT READY BUFFER", viewport.ToString());
                return false;
            }

            return api.OnBeginClip(viewport);
        }

        public bool BeginDraw()
        {
            if(isInit == false)
            {
                log.Assert("BeginDraw() : NOT INITIALIZED");
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer)
            {
                log.Warning("BeginDraw() : ALREADY CALLED");
                return true;
            }

            if (api.OnBeginDraw() == false)
                return false;

            isReadyBuffer = true;
            //tickCount = GetTickCount();
            return true;
        }

        public bool Clear(Color color)
        {
            if (isInit == false)
            {
                log.Assert("Clear({0}) : NOT INITIALIZED", color.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer)
            {
                log.Warning("Clear({0}) : LET INVALIDATE BUFFER", color.ToString());
                if (this.EndDraw() == false)
                    return false;
            }

            if (api.OnClear(color) == false)
                return false;

            return true;
        }

        public bool EndClip()
        {
            if (isInit == false)
            {
                log.Assert("EndClip() : NOT INITIALIZED");
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("EndClip() : NOT READY BUFFER");
                return false;
            }

            return api.OnEndClip();
        }

        public bool EndDraw()
        {
            if (isInit == false)
            {
                log.Assert("EndDraw() : NOT INITIALIZED");
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("EndDraw() : NOT READY BUFFER");
                return false;
            }

            if (api.OnEndDraw() == false)
                return false;

            isReadyBuffer = false;
            return true;
        }

        public bool Flip()
        {
            if (isInit == false)
            {
                log.Assert("Flip() : NOT INITIALIZED");
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer)
            {
                log.Assert("Flip() : NEED TO CALL <EndDraw>");
                return false;
            }

            return this.OnFlip();
        }

        public bool Flip(int x, int y)
        {
            if (isInit == false)
            {
                log.Assert("Flip({0}, {1}) : NOT INITIALIZED", x, y);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer)
            {
                log.Assert("Flip({0}, {1}) : NEED TO CALL <EndDraw>", x, y);
                return false;
            }

            return this.OnFlip();
        }

        public bool Flip(int x, int y, int cx, int cy)
        {
            if (isInit == false)
            {
                log.Assert("Flip({0}, {1}, {2}, {3}) : NOT INITIALIZED", x, y, cx, cy);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer)
            {
                log.Assert("Flip({0}, {1}, {2}, {3}) : NEED TO CALL <EndDraw>", x, y, cx, cy);
                return false;
            }

            return this.OnFlip();
        }

        public string GetIdentifier()
        {
            return api.OnGetIdentifier();
        }

        public bool Initialize(IntPtr hMainWindow)
        {
            if (isInit)
            {
                log.Warning("Initialize() : ALREADY CALLED");
                return true;
            }

            mainWindowHandle = hMainWindow;
            if (api.OnInitialize(hMainWindow) == false)
            {
                this.Uninitialize();
                return false;
            }

            isInit = true;
            return true;
        }

        public bool PutImage(ImageArgs r)
        {
            if (isInit == false)
            {
                log.Assert("PutImage({0}) : NOT INITIALIZED", r.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutImage({0}) : NOT READY BUFFER", r.ToString());
                return false;
            }

            return api.OnPutImage(r);
        }

        public bool PutLine(LineArgs r)
        {
            if (isInit == false)
            {
                log.Assert("PutLine({0}) : NOT INITIALIZED", r.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutLine({0}) : NOT READY BUFFER", r.ToString());
                return false;
            }

            if (r.PointCount < 2)
            {
                log.Assert("PutLine({0}) : NOT ENOUGH DATA", r.ToString());
                return false;
            }

            return api.OnPutLine(r);
        }

        public bool PutRect(Color cr)
        {
            if (isInit == false)
            {
                log.Assert("PutRect({0}) : NOT INITIALIZED", cr.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutRect({0}) : NOT READY BUFFER", cr.ToString());
                return false;
            }

            return api.OnPutRect(cr);
        }

        public bool PutRect(int left, int top, int right, int bottom, Color cr)
        {
            var r = new Rectangle(left, top, right, bottom);
            if (isInit == false)
            {
                log.Assert("PutRect({0}{1}) : NOT INITIALIZED", r.ToString(), cr.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutRect({0}{1}) : NOT READY BUFFER", r.ToString(), cr.ToString());
                return false;
            }

            return api.OnPutRect(r, cr);
        }

        public bool PutText(TextArgs r)
        {
            if (isInit == false)
            {
                log.Assert("PutText({0}) : NOT INITIALIZED", r.ToString());
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutText({0}) : NOT READY BUFFER", r.ToString());
                return false;
            }

            //if (r.IsEmpty())
            //    return true;

            //if (r.IsValid() == false)
            //{
            //    log.Assert("PutText(%s) : INVALID DATA", r.ToString());
            //    return false;
            //}

            if (api.OnPutText(r) == false)
                return false;

            //if (r.isDrawBoundary)
            //{
            //    var s = new LineArgs();
            //    s.AddPoint(r.left, r.top);
            //    s.AddPoint(r.right, r.top);
            //    s.AddPoint(r.right, r.bottom);
            //    s.AddPoint(r.left, r.bottom);
            //    s.AddPoint(r.left, r.top);
            //    s.SetColor(r.lineColor);
            //    s.SetWidth(r.lineWidth);
            //    if (api.OnPutLine(s) == false)
            //        return false;
            //}
            return true;
        }

        public bool ResizeBackBuffer(int width, int height)
        {
            if (isInit == false)
            {
                log.Assert("ResizeBackBuffer({0}, {1}) : NOT INITIALIZED", width, height);
                return false;
            }

            if (width <= 0 || height <= 0)
            {
                log.Assert("ResizeBackBuffer({0}, {1}) : INVALID SIZE", width, height);
                return false;
            }

            if (width == backBufferWidth && height == backBufferHeight)
            {
                log.Warning("ResizeBackBuffer({0}, {1}) : ALREADY CALLED", width, height);
                return true;
            }

            isLostDevice = !api.OnResizeBackBuffer(width, height);
            if (isLostDevice)
                return false;

            backBufferWidth = width;
            backBufferHeight = height;
            return true;
        }

        public void SetReporter(IReporter er)
        {
            log.SetReporter(er);
        }

        public void SetVisibleFrameRate(bool bSet)	// out to window caption
        {
            if (isInit == false)
            {
                log.Assert("SetVisibleFrameRate({0}) : NOT INITIALIZED", bSet ? "true" : "false");
                return;
            }

            isVisibleFrameRate = bSet;
        }
#endregion

        Engine(IMethod method)
        {
            api = method;
        }

        bool IsVisibleFrameRate()
        {
            return isVisibleFrameRate;
        }

        bool OnFlip()
        {
            return false;
        }

        bool RestoreDevice()
        {
            isLostDevice = !api.OnRestoreDevice(backBufferWidth, backBufferHeight);
            return !isLostDevice;
        }

        void Uninitialize()
        {
            mainWindowHandle = IntPtr.Zero;
            isInit = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct3D9;

using Core;

namespace Renderer
{
    class Engine : IRenderer
    {
        bool isInit = false;             // 초기화되었는가
        bool isLostDevice = false;
        bool isReadyBuffer = false;      // 그리기 준비가 되었다(BeginDraw에서 활성화, EndDraw에서 비활성화)
        bool isVisibleFrameRate = false;
        int backBufferHeight = 0;
        int backBufferWidth = 0;
        Form mainWindow = null;
        readonly IMethod api = null;
        int tickCount = 0;
        LogHelper log = new LogHelper();

        public bool BeginClip(System.Drawing.Rectangle viewport)
        {
            if (isInit == false)
            {
                log.Assert("BeginClip({0}) : NOT INITIALIZED", viewport);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("BeginClip({0}) : NOT READY BUFFER", viewport);
                return false;
            }

            return api.OnBeginClip(viewport);
        }

        public bool BeginDraw()
        {
            if (isInit == false)
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
            tickCount = Environment.TickCount;
            return true;
        }

        public bool Clear(MyColor color)
        {
            if (isInit == false)
            {
                log.Assert("Clear({0}) : NOT INITIALIZED", color);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer)
            {
                log.Warning("Clear({0}) : LET INVALIDATE BUFFER", color);
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

            return this.OnFlip(null, null, null, null);
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

            return this.OnFlip(x, y, null, null);
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

            return this.OnFlip(x, y, cx, cy);
        }

        public IntPtr GetHandle()
        {
            return this.mainWindow.Handle;
        }

        public IImage GetImage(string path, MyColor key, string identifier)
        {
            if (isInit == false)
            {
                log.Assert("GetImage({0}, {1}, {2}) : NOT INITIALIZED", path, key, identifier);
                return null;
            }

            return api.OnGetImage(path, key, identifier);
        }

        public bool Initialize(Form mainWindow)
        {
            if (isInit)
            {
                log.Warning("Initialize() : ALREADY CALLED");
                return true;
            }

            this.mainWindow = mainWindow;
            if (api.OnInitialize(this.mainWindow) == false)
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
                log.Assert("PutImage({0}) : NOT INITIALIZED", r);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutImage({0}) : NOT READY BUFFER", r);
                return false;
            }

            return api.OnPutImage(r);
        }

        public bool PutLine(LineArgs r)
        {
            if (isInit == false)
            {
                log.Assert("PutLine({0}) : NOT INITIALIZED", r);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutLine({0}) : NOT READY BUFFER", r);
                return false;
            }

            if (r.PointCount < 2)
            {
                log.Assert("PutLine({0}) : NOT ENOUGH DATA", r);
                return false;
            }

            return api.OnPutLine(r);
        }

        public bool PutRect(MyColor cr)
        {
            if (isInit == false)
            {
                log.Assert("PutRect({0}) : NOT INITIALIZED", cr);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutRect({0}) : NOT READY BUFFER", cr);
                return false;
            }

            return api.OnPutRect(cr);
        }

        public bool PutRect(int left, int top, int right, int bottom, MyColor cr)
        {
            var r = new System.Drawing.Rectangle(left, top, right - left, bottom - top);
            if (isInit == false)
            {
                log.Assert("PutRect({0}{1}) : NOT INITIALIZED", r, cr);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutRect({0}{1}) : NOT READY BUFFER", r, cr);
                return false;
            }

            return api.OnPutRect(r, cr);
        }

        public bool PutText(TextArgs r)
        {
            if (isInit == false)
            {
                log.Assert("PutText({0}) : NOT INITIALIZED", r);
                return false;
            }

            if (isLostDevice)
                return this.RestoreDevice();

            if (isReadyBuffer == false)
            {
                log.Assert("PutText({0}) : NOT READY BUFFER", r);
                return false;
            }

            if (r.IsEmpty)
                return true;

            if (r.IsValid == false)
            {
                log.Assert("PutText({0}) : INVALID DATA", r);
                return false;
            }

            if (api.OnPutText(r) == false)
                return false;

            if (r.IsDrawBoundary)
            {
                if (api.OnPutLine(r.BoundaryLine) == false)
                    return false;
            }
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
            api.SetReporter(er);
        }

        public void SetVisibleFrameRate(bool bSet)	// out to window caption
        {
            if (isInit == false)
            {
                log.Assert("SetVisibleFrameRate({0}) : NOT INITIALIZED", bSet);
                return;
            }

            isVisibleFrameRate = bSet;
        }

        public Engine(IMethod method)
        {
            api = method;
        }

        private bool OnFlip(int? px, int? py, int? pcx, int? pcy)
        {
            // Note : 버퍼를 화면에 출력할 때 원본 버퍼의 길이 비율이 윈도우 길이 비율에 따라 깨지지 않도록 조절한다
            var r = new System.Drawing.Rectangle(mainWindow.Left, mainWindow.Top, mainWindow.Width, mainWindow.Height);
            var rcCov = r;
            if (pcx.HasValue || pcy.HasValue)
            {
                var size = rcCov.Size;
                if (pcx.HasValue)
                    size.Width = rcCov.Left + pcx.Value;

                if (pcy.HasValue)
                    size.Height = rcCov.Top + pcy.Value;

                rcCov.Size = size;
            }
            else
            {
                float fWidthRate = (float)(r.Width) / backBufferWidth;
                float fHeightRate = (float)(r.Height) / backBufferHeight;

                // Note : 세로가 더 긴 경우
                float fRate = fWidthRate;
                if (fWidthRate > fHeightRate)
                    fRate = fHeightRate;

                if (fRate < 1.0f)
                {
                    var size = new System.Drawing.Size((int)(backBufferWidth * fRate), (int)(backBufferHeight * fRate));
                    rcCov.Size = size;
                }
                else
                {
                    var size = new System.Drawing.Size(backBufferWidth, backBufferHeight);
                    rcCov.Size = size;
                }

                var point = new System.Drawing.Point((r.Right - rcCov.Right) / 2, (r.Bottom - rcCov.Bottom) / 2);
                rcCov.Location = point;
            }

            var location = rcCov.Location;
            if (px.HasValue)
                location.X = px.Value - rcCov.Left;

            if (py.HasValue)
                location.Y = py.Value - rcCov.Top;
            rcCov.Location = location;

            bool bResult = api.OnFlip(rcCov);
            if (bResult)
            {
                if (this.IsVisibleFrameRate)
                {
                    int tc = Environment.TickCount - tickCount;
                    if (tc > 0)
                        mainWindow.Text = String.Format("Frame Rate = {0} FPS", 1000 / tc);
                }
            }
            else
            {
                bResult = this.RestoreDevice();
            }
            return bResult;
        }

        private bool RestoreDevice()
        {
            isLostDevice = !api.OnRestoreDevice(backBufferWidth, backBufferHeight);
            return !isLostDevice;
        }

        private void Uninitialize()
        {
            mainWindow = null;
            isInit = false;
        }

        public string Identifier
        {
            get { return api.OnGetIdentifier(); }
        }

        private bool IsVisibleFrameRate
        {
            get { return isVisibleFrameRate; }
        }
    }
}

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
    class DXImage : IImage
    {
        Texture texture = null;

        public string GetIdentifier()
        {
            return texture.DebugName;
        }

        public DXImage(Texture t, string identifier)
        {
            texture = t;
            texture.DebugName = identifier;
        }

        ~DXImage()
        {
            texture.Dispose();
        }

        public Texture Texture
        {
            get { return texture; }
        }
    }

    class MethodDX9 : IMethod
    {
        Form mainWindow = null;
        LogHelper log = new LogHelper();
        SharpDX.Rectangle rcSrc;
        Device device = null;
        Sprite sprite = null;
        Line line = null;
        Stack<Viewport> stackViewport = new Stack<Viewport>();
        Stack<SharpDX.Rectangle> stackScissor = new Stack<SharpDX.Rectangle>();

        #region From IMethod
        public bool OnBeginClip(System.Drawing.Rectangle r)
        {
            try
            {
                var vpOld = device.Viewport;
                {
                    var vp = new Viewport();
                    vp.X = r.Left;
                    vp.Y = r.Top;
                    vp.Width = r.Width;
                    vp.Height = r.Height;
                    vp.MinDepth = vpOld.MinDepth;
                    vp.MaxDepth = vpOld.MaxDepth;
                    device.Viewport = vp;
                }

                var rcOld = device.ScissorRect;
                device.ScissorRect = MyConvert.ToDX(r);

                stackViewport.Push(vpOld);
                stackScissor.Push(rcOld);
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnBeginDraw()
        {
            try
            {
                device.BeginScene();
                sprite.Begin(SpriteFlags.AlphaBlend);
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnClear(MyColor color)
        {
            try
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, MyConvert.ToDX(color), 1.0f, 0);
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnEndClip()
        {
            // Note : 둘 중 한 개라도 비어 있음 안 돼~
            if (stackViewport.Count == 0 || stackScissor.Count == 0)
            {
                log.Assert("<EndClip> must be called after <BeginClip>");
                return false;
            }

            try
            {
                var vp = stackViewport.Peek();
                stackViewport.Pop();
                device.Viewport = vp;

                var sc = stackScissor.Peek();
                stackScissor.Pop();
                device.ScissorRect = sc;
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnEndDraw()
        {
            try
            {
                sprite.End();
                device.EndScene();
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnFlip(System.Drawing.Rectangle rcDest)
        {
            try
            {
                device.Present(rcSrc, MyConvert.ToDX(rcDest));
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public string OnGetIdentifier()
        {
            return "DirectX 9.0c with SharpDX";
        }

        public IImage OnGetImage(string path, MyColor key, string identifier)
        {
            try
            {
                var image = System.Drawing.Image.FromFile(path);

                var texture = Texture.FromFile(device, path, image.Width, image.Height, 1, Usage.None, Format.Unknown, Pool.Default, Filter.Default, Filter.Default, key.ToInteger());
                return new DXImage(texture, identifier);
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return null;
            }
        }

        public bool OnInitialize(Form mainWindow)
        {
            this.mainWindow = mainWindow;
            return true;
        }

        public bool OnPutImage(ImageArgs r)
        {
            try
            {
                Matrix matOld = sprite.Transform;
                sprite.Transform = Matrix.Scaling(r.sx, r.sy, r.sz) * Matrix.Translation(r.px, r.py, r.pz);

                // Note : C#용 컬러 구조체에 대해 다시 생각해보자
                var image = r.image as DXImage;
                sprite.Draw(image.Texture, ColorBGRA.FromRgba(0xFFFFFFFF));
                sprite.Flush();
                sprite.Transform = matOld;
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnPutLine(LineArgs r)
        {
            try
            {
                // Note : SetWidth 는 Begin ~ End 사이에서 하면 안 된다
                line.Width = r.LineWidth;

                List<Vector2> v = new List<Vector2>();
                foreach (var pt in r.Points)
                    v.Add(new Vector2(pt.X, pt.Y));

                // Note : Begin ~ End 는 생략이 가능하다. Draw 내부에서 자동으로 호출해준다
                //          여러 선을 한꺼번에 그릴 경우에는 오버헤드를 피하기 위해 명시적으로 선언해줄 필요가 있다
                line.Draw(v.ToArray(), MyConvert.ToDX(r.LineColor));
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnPutRect(MyColor cr)
        {
            try
            {
                var surface = device.GetRenderTarget(0);
                var desc = surface.Description;
                var rc = this.GetClipRect(new Rectangle(0, 0, desc.Width, desc.Height));

                device.ColorFill(surface, rc, MyConvert.ToDX(cr));
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnPutRect(System.Drawing.Rectangle rc, MyColor cr)
        {
            try
            {
                var surface = device.GetRenderTarget(0);
                var rc2 = this.GetClipRect(MyConvert.ToDX(rc));

                device.ColorFill(surface, rc2, MyConvert.ToDX(cr));
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnPutText(TextArgs r)
        {
            try
            {
                var vp = device.Viewport;
                var rc = new Rectangle(vp.X + r.left, vp.Y + r.top, vp.X + r.right, vp.Y + r.bottom);
                var font = new SharpDX.Direct3D9.Font(device, r.font);

                font.DrawText(null, r.text, rc, MyConvert.ToDX(r.format), MyConvert.ToDX(r.textColor));
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public bool OnResizeBackBuffer(int width, int height)
        {
            try
            {
                this.ReleaseDevice();

                var pp = new PresentParameters(width, height);
                pp.Windowed = true;
                pp.SwapEffect = SwapEffect.Discard; // for window mode
                pp.BackBufferFormat = Format.Unknown;
                pp.EnableAutoDepthStencil = true;
                pp.AutoDepthStencilFormat = Format.D16;

                device = new Device(new Direct3D(), 0, DeviceType.Hardware, mainWindow.Handle, CreateFlags.HardwareVertexProcessing, pp);
                device.SetRenderState(RenderState.ScissorTestEnable, true); // for clip rect

                sprite = new Sprite(device);
                line = new Line(device);
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            rcSrc = new Rectangle(0, 0, width, height);
            return true;
        }

        public bool OnRestoreDevice(int width, int height)
        {
            try
            {
                if (this.OnResizeBackBuffer(width, height) == false)
                    return false;
            }
            catch (SharpDXException e)
            {
                log.Error(e.ToString());
                return false;
            }
            return true;
        }

        public void SetReporter(IReporter r)
        {
            log.SetReporter(r);
        }
        #endregion

        private void ReleaseDevice()
        {
            if (line != null)
            {
                line.Dispose();
                line = null;
            }

            if (sprite != null)
            {
                sprite.Dispose();
                sprite = null;
            }

            if (device != null)
            {
                device.Dispose();
                device = null;
            }
        }

        private SharpDX.Rectangle GetClipRect(SharpDX.Rectangle r)
        {
            var vp = device.Viewport;
            var rc = device.ScissorRect;

            r.Inflate(vp.X, vp.Y);
            r.Intersects(rc);
            return r;
        }
    }
}

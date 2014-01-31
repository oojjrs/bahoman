using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;

namespace Renderer
{
    public class TextArgs
    {
        private bool isDrawBoundary = false;
        private Vector3f pos = new Vector3f();
        private Size size = new Size();
        private System.Windows.Forms.TextFormatFlags format = 0;
        private int lineWidth = 1;
        private string text = "";
        private System.Drawing.Font font = null;
        private MyColor lineColor = new MyColor();
        private MyColor textColor = new MyColor();

        public override string ToString()
        {
            string s;
            if (font == null)
            {
                s = String.Format("'{0}'Font(None){1}{2}{3}Format(0x{4})",
                    text, textColor.ToString(), pos.ToString(), size.ToString(), format.ToString("X"));
            }
            else
            {
                s = String.Format("'{0}'Font({1}){2}{3}{4}Format(0x{5})",
                    text, font.ToString(), textColor.ToString(), pos.ToString(), size.ToString(), format.ToString("X"));
            }
            return s;
        }

        public static TextArgs Create(string text, System.Drawing.Font font)
        {
            var t = new TextArgs();
            t.Font = font;
            t.text = text;
            return t;
        }

        public int Bottom
        {
            get { return this.Top + size.Height; }
            set { size.Height = value - this.Top; }
        }

        public LineArgs BoundaryLine
        {
            get
            {
                var s = new LineArgs();
                s.AddPoint(this.LeftTop);
                s.AddPoint(this.RightTop);
                s.AddPoint(this.RightBottom);
                s.AddPoint(this.LeftBottom);
                s.AddPoint(this.LeftTop);
                s.LineColor = this.LineColor;
                s.LineWidth = this.LineWidth;
                return s;
            }
        }

        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public System.Windows.Forms.TextFormatFlags Format
        {
            get { return format; }
            set { format = value; }
        }

        public int Height
        {
            get { return size.Height; }
            set { size.Height = value; }
        }

        public bool IsDrawBoundary
        {
            get { return isDrawBoundary; }
            set { isDrawBoundary = value; }
        }

        public bool IsEmpty
        {
            get { return size.IsEmpty; }
        }

        public bool IsValid
        {
            get { return font != null && text != ""; }
        }

        public int Left
        {
            get { return (int)pos.X; }
            set { pos.X = value; }
        }

        public Point LeftBottom
        {
            get { return new Point(this.Left, this.Bottom); }
        }

        public Point LeftTop
        {
            get { return new Point(this.Left, this.Top); }
        }

        public MyColor LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public Vector3f Location
        {
            get { return pos; }
            set { pos = value; }
        }

        public Rectangle Rect
        {
            get { return new Rectangle(this.LeftTop, this.Size); }
            set { this.Location = new Vector3f(value.X, 0, value.Y); this.Size = value.Size; }
        }

        public int Right
        {
            get { return this.Left + size.Width; }
            set { size.Width = value - this.Left; }
        }

        public Point RightBottom
        {
            get { return new Point(this.Right, this.Bottom); }
        }

        public Point RightTop
        {
            get { return new Point(this.Right, this.Top); }
        }

        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public MyColor TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public int Top
        {
            get { return (int)pos.Z; }
            set { pos.Z = value; }
        }

        public int Width
        {
            get { return size.Width; }
            set { size.Width = value; }
        }
    }
}

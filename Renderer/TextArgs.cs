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
        private Point pos = new Point();
        private Size size = new Size();
        private System.Windows.Forms.TextFormatFlags format = 0;
        private int lineWidth = 1;
        private string text = "";
        private System.Drawing.Font font = null;
        private MyColor lineColor = new MyColor();
        private MyColor textColor = new MyColor();

        public override string ToString()
        {
            var r = new System.Drawing.Rectangle(pos, size);

            string s;
            if (font == null)
            {
                s = String.Format("'{0}'Font(None){1}{2}Format(0x{3})",
                    text, textColor.ToString(), r.ToString(), format.ToString("X"));
            }
            else
            {
                s = String.Format("'{0}'Font({1}){2}{3}Format(0x{4})",
                    text, font.ToString(), textColor.ToString(), r.ToString(), format.ToString("X"));
            }
            return s;
        }

        public int Bottom
        {
            get { return pos.Y + size.Height; }
            set { size.Height = value - pos.Y; }
        }

        public LineArgs BoundaryLine
        {
            get
            {
                var s = new LineArgs();
                s.AddPoint(this.Location);
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
        }

        public bool IsEmpty
        {
            get { return pos.IsEmpty || size.IsEmpty; }
        }

        public bool IsValid
        {
            get { return font != null && text != ""; }
        }

        public int Left
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public Point LeftBottom
        {
            get { return new Point(this.Left, this.Bottom); }
        }

        public Point LeftTop
        {
            get { return pos; }
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

        public Point Location
        {
            get { return pos; }
            set { pos = value; }
        }

        public Rectangle Rect
        {
            get { return new Rectangle(pos, size); }
            set { this.Location = value.Location; this.Size = value.Size; }
        }

        public int Right
        {
            get { return pos.X + size.Width; }
            set { size.Width = value - pos.X; }
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
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        public int Width
        {
            get { return size.Width; }
            set { size.Width = value; }
        }
    }
}

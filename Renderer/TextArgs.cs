using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public class TextArgs
    {
        public bool isDrawBoundary = false;
        public int left = 0;
        public int top = 0;
        public int right = 0;
        public int bottom = 0;
        public int format = 0;
        public int lineWidth = 1;
        public string text = "";
        public Font font = null;
        public Color lineColor = Color.Black;
        public Color textColor = Color.Black;

        public override string ToString()
        {
            var s = new StringBuilder();
            var r = new Rectangle(left, top, right - left, bottom - top);

            if (font == null)
            {
                s.AppendFormat("'{0}'Font(None){1}{2}Format(0x{3})",
                    text, textColor.ToString(), r.ToString(), format.ToString("X"));
            }
            else
            {
                s.AppendFormat("'{0}'Font({1}){2}{3}Format(0x{4})",
                    text, font.ToString(), textColor.ToString(), r.ToString(), format.ToString("X"));
            }
            return s.ToString();
        }

        public bool IsEmpty()
        {
            return (left == 0 && right == 0) || (top == 0 && bottom == 0);
        }

        public bool IsValid()
        {
            return font != null && text != "";
        }
    }
}

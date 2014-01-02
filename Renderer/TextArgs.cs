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
        //private bool isDrawBoundary = false;
        private int left = 0;
        private int top = 0;
        private int right = 0;
        private int bottom = 0;
        private int format = 0;
        //private int lineWidth = 1;
        private string text = "";
        private Font font = null;
        private Color lineColor = Color.Black;
        private Color textColor = Color.Black;

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
    }
}

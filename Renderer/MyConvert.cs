using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D9;

using Core;

namespace System
{
    public static class MyConvert
    {
        public static SharpDX.Rectangle ToDX(System.Drawing.Rectangle v)
        {
            return new SharpDX.Rectangle(v.X, v.Y, v.Width, v.Height);
        }

        public static SharpDX.ColorBGRA ToDX(MyColor v)
        {
            return SharpDX.ColorBGRA.FromRgba(v.ToInteger());
        }

        public static SharpDX.Direct3D9.FontDrawFlags ToDX(System.Windows.Forms.TextFormatFlags v)
        {
            var ret = new SharpDX.Direct3D9.FontDrawFlags();

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.HorizontalCenter))
                ret |= FontDrawFlags.Center;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.Right))
                ret |= FontDrawFlags.Right;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.VerticalCenter))
                ret |= FontDrawFlags.VerticalCenter;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.Bottom))
                ret |= FontDrawFlags.Bottom;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.WordBreak))
                ret |= FontDrawFlags.WordBreak;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.SingleLine))
                ret |= FontDrawFlags.SingleLine;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.ExpandTabs))
                ret |= FontDrawFlags.ExpandTabs;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.NoClipping))
                ret |= FontDrawFlags.NoClip;

            if (v.HasFlag(System.Windows.Forms.TextFormatFlags.RightToLeft))
                ret |= FontDrawFlags.RtlReading;

            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class MyColor
    {
        public enum Type
        {
            RGB,
            ARGB,
            ABGR,
            RGBA,
            BGRA,
        }

        private byte r;
        private byte g;
        private byte b;
        private byte a;

        public override string ToString()
        {
            return String.Format("Color(R{0}G{1}B{2}A{3}", r, g, b, a);
        }

        public MyColor()
        {
            this.Set(this.Join(0, 0, 0, 255));
        }

        public MyColor(System.Drawing.Color color)
        {
            this.Set(color.ToArgb(), Type.ARGB);
        }

        public MyColor(int color, Type t = Type.RGBA)
        {
            this.Set(color, t);
        }

        public MyColor(byte r, byte g, byte b, byte a)
        {
            this.Set(this.Join(r, g, b, a));
        }

        public void Set(int color, Type t = Type.RGBA)
        {
            switch (t)
            {
                case Type.RGB:
                    r = (byte)(color & 0x000000FF);
                    g = (byte)((color & 0x0000FF00) >> 8);
                    b = (byte)((color & 0x00FF0000) >> 16);
                    a = 255;
                    break;
                case Type.ARGB:
                    a = (byte)(color & 0x000000FF);
                    r = (byte)((color & 0x0000FF00) >> 8);
                    g = (byte)((color & 0x00FF0000) >> 16);
                    b = (byte)((color & 0xFF000000) >> 24);
                    break;
                case Type.ABGR:
                    a = (byte)(color & 0x000000FF);
                    b = (byte)((color & 0x0000FF00) >> 8);
                    g = (byte)((color & 0x00FF0000) >> 16);
                    r = (byte)((color & 0xFF000000) >> 24);
                    break;
                case Type.RGBA:
                    r = (byte)(color & 0x000000FF);
                    g = (byte)((color & 0x0000FF00) >> 8);
                    b = (byte)((color & 0x00FF0000) >> 16);
                    a = (byte)((color & 0xFF000000) >> 24);
                    break;
                case Type.BGRA:
                    b = (byte)(color & 0x000000FF);
                    g = (byte)((color & 0x0000FF00) >> 8);
                    r = (byte)((color & 0x00FF0000) >> 16);
                    a = (byte)((color & 0xFF000000) >> 24);
                    break;
            }
        }

        public int ToInteger(Type t = Type.RGBA)
        {
            switch (t)
            {
                case Type.RGB:
                    return this.Join(r, g, b, 255);
                case Type.ARGB:
                    return this.Join(a, r, g, b);
                case Type.ABGR:
                    return this.Join(a, b, g, r);
                case Type.RGBA:
                    return this.Join(r, g, b, a);
                case Type.BGRA:
                    return this.Join(b, g, r, a);
            }
            return -1;
        }

        private int Join(int a, int b, int c, int d)
        {
            return (a & 0xFF)
                | ((b & 0xFF) << 8)
                | ((c & 0xFF) << 16)
                | ((d & 0xFF) << 24)
                ;
        }

        public System.Drawing.Color SystemColor
        {
            get { return System.Drawing.Color.FromArgb(a, r, g, b); }
        }
    }
}

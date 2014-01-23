using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Core;
using Renderer;

namespace bball
{
    static class ImageFactory
    {
        public static readonly MyColor defaultColorKey = new MyColor();

        private static IRenderer r;
        private static Dictionary<string, IImage> cached = new Dictionary<string, IImage>();

        public static IImage Create(string subPath)
        {
            var identifier = Path.GetFileNameWithoutExtension(subPath);
            IImage ret;
            if(cached.TryGetValue(identifier, out ret))
                return ret;

            ret = r.GetImage(subPath, defaultColorKey, identifier);
            cached[identifier] = ret;
            return ret;
        }

        public static IRenderer Renderer
        {
            set { r = value; }
        }
    }
}

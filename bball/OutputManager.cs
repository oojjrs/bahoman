using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    class OutputManager
    {
        private static List<IDrawable> objects = new List<IDrawable>();

        public static void Add(IDrawable obj)
        {
            foreach (var o in objects)
            {
                if (o == obj)
                    return;
            }

            objects.Add(obj);
        }

        public static void OutputAll(IRenderer r)
        {
            foreach (var o in objects)
                o.OnDraw(r);
        }

        public static void Remove(IDrawable obj)
        {
            objects.Remove(obj);
        }
    }
}

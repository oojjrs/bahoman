﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    static class OutputManager
    {
        private static List<IOutputModel> objects = new List<IOutputModel>();

        public static void Add(IOutputModel obj)
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

        public static void Remove(IOutputModel obj)
        {
            objects.Remove(obj);
        }

        public static void RemoveAll()
        {
            objects.Clear();
        }

        public static void UpdateAll()
        {
            foreach (var o in objects)
                o.OnUpdate();
        }
    }
}

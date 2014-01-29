using System;
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

        // Note : 아직 3D 좌표계가 완전하지 않으므로 그리는 순서가 중요하다. 적당히 마무리 짓고 딴 거부터 하자
        public static void MoveToFirst(IOutputModel obj)
        {
            OutputManager.Remove(obj);
            objects.Insert(0, obj);
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

        public static void UpdateAll()
        {
            foreach (var o in objects)
                o.OnUpdate();
        }
    }
}

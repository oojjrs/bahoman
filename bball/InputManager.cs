using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using AI;

namespace bball
{
    static class InputManager
    {
        private static List<IMouseHandler> mouses = new List<IMouseHandler>();

        public static void Add(IMouseHandler h)
        {
            foreach (var m in mouses)
            {
                if (m == h)
                    return;
            }

            mouses.Insert(0, h);
        }

        public static void Remove(IMouseHandler h)
        {
            mouses.Remove(h);
        }

        public static void RemoveAll()
        {
            mouses.Clear();
        }

        public static bool OnMouseMove(MouseEventArgs e)
        {
            return false;
        }

        public static bool OnButtonDown(MouseEventArgs e)
        {
            return false;
        }

        public static bool OnButtonUp(MouseEventArgs e)
        {
            return false;
        }

        public static bool OnButtonClick(MouseEventArgs e)
        {
            var target = InputManager.GetMouseTarget(e);
            if (target == null)
                return false;

            var args = new MouseArgs();
            args.e = e;
            args.Modifier = Keys.None;  // 추후 추가
            args.Location = CourtPos.FromCoord(e.X, 0.0f, e.Y);

            return target.OnMouse(args);
        }

        public static bool OnMouseWheel(int flags, int delta, int x, int y)
        {
            return false;
        }

        private static IMouseHandler GetMouseTarget(MouseEventArgs e)
        {
            foreach (var m in mouses)
            {
                if (m.Zone.Contains(e.Location))
                    return m;
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    abstract class Object : IOutputModel
    {
        public abstract void OnDraw(IRenderer r);
        public abstract void OnUpdate();

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Object l, Object r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Object l, Object r)
        {
            return !l.Equals(r);
        }

        public bool Visible
        {
            set
            {
                if (value)
                    OutputManager.Add(this);
                else
                    OutputManager.Remove(this);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    abstract class Object : IDrawable
    {
        public abstract void OnDraw(IRenderer r);

        public Object()
        {
            OutputManager.Add(this);
        }

        ~Object()
        {
            OutputManager.Remove(this);
        }
    }
}

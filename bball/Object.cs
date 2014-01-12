using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    class Object : IDrawable
    {
        public virtual void OnDraw(IRenderer r)
        {
        }

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

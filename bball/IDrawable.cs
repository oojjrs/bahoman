using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    interface IDrawable
    {
        void OnDraw(IRenderer r);
    }
}

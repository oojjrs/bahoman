using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;

namespace bball
{
    interface IOutputModel
    {
        void OnDraw(IRenderer r);
        void OnUpdate();
    }
}

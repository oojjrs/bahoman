using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AI;
using Renderer;

namespace bball
{
    abstract class Object : IOutputModel
    {
        public abstract void OnDraw(IRenderer r);
        public abstract void OnUpdate();

        public abstract bool IsClick(CourtPos location);

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

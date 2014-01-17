﻿using System;
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

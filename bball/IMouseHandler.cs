using System;
using System.Drawing;

namespace bball
{
    interface IMouseHandler
    {
        bool OnMouse(MouseArgs e);
        Rectangle Zone { get; }
    }
}

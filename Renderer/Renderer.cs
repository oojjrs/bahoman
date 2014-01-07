using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public enum Type
    {
        //Software,
        Direct3D9,
        Stream,
    }

    public static class Container
    {
        private static IRenderer viaDX = new Engine(new MethodDX9());
        private static IRenderer viaText = new Engine(new MethodStream());

        public static IRenderer GetInterface(Type t)
        {
            switch (t)
            {
                case Type.Direct3D9:
                    return viaDX;
                case Type.Stream:
                    return viaText;
            }
            return viaDX;
        }
    }
}

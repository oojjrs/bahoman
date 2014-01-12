using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;

namespace Renderer
{
    class MyExtractor
    {
        public static SharpDX.Matrix GetScaleMatrix(ImageArgs r)
        {
            return Matrix.Scaling(r.ScaleX, r.ScaleY, r.ScaleZ);
        }

        public static SharpDX.Matrix GetTranslationMatrix(ImageArgs r)
        {
            return Matrix.Translation(r.PosX, r.PosY, r.PosZ);
        }
    }
}

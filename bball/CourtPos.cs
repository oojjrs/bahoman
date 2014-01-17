using System;
using System.Drawing;
using Core;
using Physics;

namespace bball
{
    class CourtPos
    {
        // Note : (0, 0) = Center of Court
        private Vector3f pos = new Vector3f();

        public static readonly CourtPos Center = new CourtPos();

        public CourtPos()
        {
        }

        public CourtPos(int x, int y, int z)
        {
            pos.Set(x, y, z);
        }

        public CourtPos(float x, float y, float z)
        {
            pos.Set(x, y, z);
        }

        public Vector3f GlobalLocation
        {
            get { return Court.LogicalCoordToPhysicalCoord(pos); }
        }
    }
}

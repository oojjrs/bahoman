using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AI
{
    public class TargetInfo
    {
        public enum Type
        {
            None,
            AttackPosition,
            Ball,
            Goal,
            ScreenPosition,
        }

        private Type type = Type.None;
        private CourtPos pos = new CourtPos();

        public Type TargetType
        {
            get { return type; }
            set { type = value; }
        }

        public CourtPos Position
        {
            get { return pos; }
            set { pos = value; }
        }
    }
}

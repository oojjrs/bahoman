using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;

namespace AI
{
    public struct PlayerAIResult
    {
        public PlayerState State;
        public CourtPos TargetLocation;

        public override string ToString()
        {
            var ret = new StringBuilder("\r\n");
            ret.AppendFormat("{0}\r\n", this.GetType());
            ret.AppendFormat("    State : {0}\r\n", State);
            ret.AppendFormat("    TargetLocation : {0}\r\n", TargetLocation);
            return ret.ToString();
        }
    }
}

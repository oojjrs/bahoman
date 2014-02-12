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
        public CourtPos BallDirection;
        public float BallVelocity;

        public override string ToString()
        {
            var ret = new StringBuilder("\r\n");
            ret.AppendFormat("{0}\r\n", this.GetType());
            ret.AppendFormat("    State : {0}\r\n", State);
            ret.AppendFormat("    TargetLocation : {0}\r\n", TargetLocation);
            ret.AppendFormat("    BallDirection : {0}\r\n", BallDirection);
            ret.AppendFormat("    BallVelocity : {0}\r\n", BallVelocity);
            return ret.ToString();
        }
    }
}

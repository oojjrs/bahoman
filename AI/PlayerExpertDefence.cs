using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;

namespace AI
{
    partial class PlayerExpert : IPlayerAIType
    {
        private PlayerAIResult StateDefence(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = false;

            CourtPos v = new CourtPos();
            if (factor.GetValue("HomePositionLocation", ref v))
            {
                ret.State = PlayerState.Move;
                ret.TargetLocation = v;
            }
            return ret;
        }
    }
}

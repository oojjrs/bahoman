using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;

namespace AI
{
    partial class PlayerExpert : IPlayerAIType
    {
        private PlayerAIResult StateLooseBall(PropertyBag factor)
        {
            if (factor.IsFlagOn("PlayerState.Move"))
                return this.StateLooseBallMove(factor);
            else if (factor.IsFlagOn("PlayerState.FindBall"))
                return this.StateLooseBallFindBall(factor);
            else if (factor.IsFlagOn("PlayerState.Ready"))
                return this.StateLooseBallReady(factor);
            else if (factor.IsFlagOn("PlayerState.Free"))
                return this.StateLooseBallFree(factor);
            else if (factor.IsFlagOn("PlayerState.Stand"))
                return this.StateLooseBallStand(factor);
            else if (factor.IsFlagOn("PlayerState.Rebound"))
                return this.StateLooseBallRebound(factor);
            else
                throw new Exception("루즈볼 상태에서는 불가능한 플레이어 상태입니다.");
        }

        private PlayerAIResult StateLooseBallMove(PropertyBag factor)
        {
            return this.StateLooseBallReady(factor);
        }

        private PlayerAIResult StateLooseBallFindBall(PropertyBag factor)
        {
            var distanceToBall = this.GetDistanceToBall(factor);
            if (distanceToBall < 10 * factor.GetFactor("CatchableReach"))
            {
                var ret = new PlayerAIResult();
                ret.State = PlayerState.CatchBall;
                return ret;
            }

            return this.StateLooseBallReady(factor);
        }

        private PlayerAIResult StateLooseBallReady(PropertyBag factor)
        {
            var ret = new PlayerAIResult();

            CourtPos ploc = new CourtPos();
            factor.GetValue("PlayerLocation", ref ploc);

            AwarenessInfo info = new AwarenessInfo();
            factor.GetValue("AwarenessInfo", ref info);

            var bloc = info.BallInfo.Location;
            var shortestHome = 10000.0f;
            var shortestAway = 10000.0f;
            this.GetShortestDistanceTo(info, bloc, ref shortestHome, ref shortestAway);

            var myDistance = ploc.DistanceTo(bloc);
            if (myDistance < shortestHome && myDistance < shortestAway)
            {
                ret.State = PlayerState.FindBall;
                ret.TargetLocation = bloc;
            }
            else
            {
                var target = new CourtPos();
                if (shortestHome < shortestAway)
                    factor.GetValue("AwayPositionLocation", ref target);
                else
                    factor.GetValue("HomePositionLocation", ref target);

                ret.State = PlayerState.Move;
                ret.TargetLocation = target;
            }
            return ret;
        }

        private PlayerAIResult StateLooseBallFree(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = true;
            return ret;
        }

        private PlayerAIResult StateLooseBallStand(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = true;
            return ret;
        }

        private PlayerAIResult StateLooseBallRebound(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = true;
            return ret;
        }
    }
}

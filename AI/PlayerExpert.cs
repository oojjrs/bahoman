using System;
using System.Collections.Generic;
using System.Drawing;

using Core;

namespace AI
{
    partial class PlayerExpert : IPlayerAIType
    {
        private static PlayerExpert instance = new PlayerExpert();
        private LogHelper log = new LogHelper();

        public PlayerAIResult Determine(PropertyBag factor)
        {
            log.AITrace(factor.ToString());

            PlayerAIResult ret;
            if (factor.IsFlagOn("TeamState.LooseBall"))
            {
                ret = this.StateLooseBall(factor);
            }
            else if (factor.IsFlagOn("TeamState.Attack"))
            {
                ret = this.StateAttack(factor);
            }
            else if (factor.IsFlagOn("TeamState.Defence"))
            {
                ret = this.StateDefence(factor);
            }
            else
            {
                throw new Exception { };
            }

            log.AITrace(ret.ToString());
            return ret;
        }

        private void GetShortestDistanceTo(AwarenessInfo info, CourtPos target, ref float home, ref float away)
        {
            foreach (var pi in info.PlayerAwarenessInfos)
            {
                var distance = pi.Location.DistanceTo(target);
                if (pi.IsTeammate)
                {
                    if (distance < home)
                        home = distance;
                }
                else
                {
                    if (distance < away)
                        away = distance;
                }
            }
        }

        public void SetReporter(IReporter er)
        {
            log.SetReporter(er);
        }

        private int GetShootingPoint(CourtPos bp, CourtPos ep)
        {
            //슛을 쏠지 말지 결정하는 팩터들을 수치화
            float distancefromRing = bp.DistanceTo(ep);

            //일단 골대 근처에 있으면 100점으로 리턴
            if (60 > (int)distancefromRing)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public static PlayerExpert Instance
        {
            get { return instance; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using Physics;

using Core;

namespace AI
{
    class PlayerExpert : IPlayerAIType
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

        private PlayerAIResult StateLooseBall(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            AwarenessInfo awarenessInfo = new AwarenessInfo();
            factor.GetValue("AwarenessInfo", ref awarenessInfo);

            CourtPos ploc = new CourtPos();
            factor.GetValue("PlayerLocation", ref ploc);

            CourtPos bloc = awarenessInfo.BallInfo.Location;

            CourtPos[] tlocs;
            factor.GetValues("TeammateLocation", out tlocs, false);

            var playerDistance = ploc.DistanceTo(bloc);
            ret.State = PlayerState.FindBall;
            ret.TargetLocation = bloc;

            foreach (var teammate in awarenessInfo.PlayerAwarenessInfos)
            {
                if (teammate.IsTeammate && playerDistance > teammate.Location.DistanceTo(bloc))
                {
                    CourtPos posLoc = new CourtPos();
                    factor.GetValue("PositionLocation", ref posLoc);

                    ret.State = PlayerState.Free;
                    ret.TargetLocation = posLoc;
                    break;
                }
            }
            return ret;
        }

        private PlayerAIResult StateAttack(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            if (factor.IsFlagOn("PlayerState.Free"))
            {
                CourtPos posLoc = new CourtPos();
                factor.GetValue("PositionLocation", ref posLoc);

                ret.State = PlayerState.Free;
                ret.TargetLocation = posLoc;
                if (factor.IsFlagOn("Ball") && !factor.IsFlagOn("IsThrower"))
                {
                    CourtPos ploc = new CourtPos();
                    factor.GetValue("PlayerLocation", ref ploc);

                    CourtPos bloc = new CourtPos();
                    factor.GetValue("BallLocation", ref bloc);

                    var distanceToBall = ploc.DistanceTo(bloc);
                    if (distanceToBall < 10)
                    {
                        ret.State = PlayerState.CatchBall;
                        ret.TargetLocation = bloc;
                    }
                }
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Pass"))
            {
                ret.State = PlayerState.Pass;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.FindBall"))
            {
                CourtPos posLoc = new CourtPos();
                factor.GetValue("BallLocation", ref posLoc);

                ret.State = PlayerState.Free;
                ret.TargetLocation = posLoc;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Dribble"))
            {
                CourtPos ploc = new CourtPos();
                factor.GetValue("PlayerLocation", ref ploc);

                CourtPos rloc = new CourtPos();
                factor.GetValue("RingLocation", ref rloc);

                var distanceToRing = ploc.DistanceTo(rloc);

                AwarenessInfo awarenessInfo = new AwarenessInfo();
                factor.GetValue("AwarenessInfo", ref awarenessInfo);

                float middleShot = 0.0f;
                factor.GetValue("MiddleShotAbility", ref middleShot);

                float maxScore = 0.0f;
                PlayerAwareness maxTarget = null;
                foreach (var pi in awarenessInfo.PlayerAwarenessInfos)
                {
                    if (pi.IsTeammate == false)
                        continue;

                    // Note : 패스적합성판단점수 = (대상위치에서의)슛성공률 + (대상에게로의)패스 성공률
                    //        지금은 상대평가 자료가 없으므로 계산은 나의 성공률에 기반하여 계산한다.
                    //        일단 무조건 미들샷(거리가 150픽셀 안쪽일 때 1.0 팩터가 100% 성공률)을 기준으로 계산
                    //        패스 성공률은 사이에 상대팀을 체크해야 하므로 아직 넣지 않음
                    float score = 150.0f / Math.Max(pi.Location.DistanceTo(rloc), 150.0f) * middleShot;
                    if (score > maxScore)
                    {
                        maxScore = score;
                        maxTarget = pi;
                    }
                }

                if (maxTarget != null)
                {
                    ret.State = PlayerState.Pass;
                    ret.TargetLocation = maxTarget.Location + maxTarget.Direction * (ploc.DistanceTo(maxTarget.Location) / 5.0f);
                    return ret;
                }
                else
                {
                    // Note : 슛을 하거나 직접 드리블한다.
                    ret.State = PlayerState.Dribble;
                    ret.TargetLocation = rloc;
                    return ret;
                }
            }
            else if (factor.IsFlagOn("PlayerState.Shoot"))
            {
                ret.State = PlayerState.Rebound;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Rebound"))
            {
                ret.State = PlayerState.Rebound;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.CatchBall"))
            {
                CourtPos bloc = new CourtPos();
                factor.GetValue("BallLocation", ref bloc);

                ret.State = PlayerState.CatchBall;
                ret.TargetLocation = bloc;
                return ret;
            }
            else
            {
                throw new Exception { };
            }
        }

        private PlayerAIResult StateDefence(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            CourtPos v = new CourtPos();
            if (factor.GetValue("TargetLocation", ref v))
            {
                ret.State = PlayerState.Move;
                ret.TargetLocation = v;
            }
            return ret;
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

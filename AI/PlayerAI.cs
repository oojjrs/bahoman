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

        private PlayerAIResult StateLooseBall(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = false;

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

        private PlayerAIResult StateAttack(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = false;

            if (factor.IsFlagOn("PlayerState.Free"))
            {
                CourtPos posLoc = new CourtPos();
                factor.GetValue("AwayPositionLocation", ref posLoc);

                CourtPos ploc = new CourtPos();
                factor.GetValue("PlayerLocation", ref ploc);

                ret.State = PlayerState.Stand;
                ret.TargetLocation = ploc;
                if (factor.IsFlagOn("Ball") && !factor.IsFlagOn("IsThrower"))
                {
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
                CourtPos posLoc = new CourtPos();
                factor.GetValue("AwayPositionLocation", ref posLoc);

                ret.State = PlayerState.Free;
                ret.TargetLocation = posLoc;
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
                    // Note : ballFloatingTime(t)의 단위가 프레임(tick)이므로 시간(s)으로서는 정확하지 않다. 추후 수정 필요
                    float ballFloatingTime = 50.0f;
                    ret.State = PlayerState.Pass;
                    ret.BallVelocity = maxTarget.Location.DistanceTo(ploc) / ballFloatingTime;
                    ret.BallDirection = (maxTarget.Location + maxTarget.Direction * ballFloatingTime).Normalize;
                }
                else
                {
                    // Note : 슛을 하거나 직접 드리블한다.
                    if (ploc.DistanceTo(rloc) < 150.0f)
                    {
                        ret.State = PlayerState.Shoot;
                        ret.TargetLocation = rloc;
                    }
                    else
                    {
                        ret.State = PlayerState.Dribble;
                        ret.TargetLocation = rloc;
                    }
                }
                return ret;
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
                CourtPos ploc = new CourtPos();
                factor.GetValue("PlayerLocation", ref ploc);

                ret.State = PlayerState.CatchBall;
                ret.TargetLocation = ploc;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Move"))
            {
                CourtPos ploc = new CourtPos();
                factor.GetValue("PlayerLocation", ref ploc);

                CourtPos dir = new CourtPos();
                factor.GetValue("Direction", ref dir);

                AwarenessInfo info = new AwarenessInfo();
                factor.GetValue("AwarenessInfo", ref info);

                var iloc = PhysicsEngine.GetIntersectLocation(
                    ploc.Vector,
                    (ploc + dir * 1000.0f).Vector,
                    info.BallInfo.Location.Vector,
                    (info.BallInfo.Location + info.BallInfo.Direction * 1000.0f).Vector);

                var myDistance = 0.0f;
                var ballDistance = 0.0f;
                if (iloc.Length() != 0.0f)
                {
                    myDistance = iloc.Distance(ploc.Vector);
                    ballDistance = iloc.Distance(info.BallInfo.Location.Vector);
                }

                if (ballDistance < myDistance * info.BallInfo.Velocity)
                {
                    ret.State = PlayerState.FindBall;
                    ret.TargetLocation = ploc + dir * 1000.0f;
                }
                else
                {
                    ret.UsePreviousResult = true;
                }
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Stand"))
            {
                ret.UsePreviousResult = true;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Stand"))
            {
                AwarenessInfo info = new AwarenessInfo();
                factor.GetValue("AwarenessInfo", ref info);

                foreach (var pi in info.PlayerAwarenessInfos)
                {
                }

                ret.State = PlayerState.Stand;
                return ret;
            }
            else
            {
                throw new Exception("새로운 상태에 대한 핸들러가 필요합니다");
            }
        }

        private PlayerAIResult StateDefence(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            ret.UsePreviousResult = false;

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

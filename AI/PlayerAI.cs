﻿using System;
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
            CourtPos ploc = new CourtPos();
            factor.GetValue("PlayerLocation", ref ploc);

            CourtPos bloc = new CourtPos();
            factor.GetValue("BallLocation", ref bloc);

            CourtPos[] tlocs;
            factor.GetValues("TeammateLocation", out tlocs, false);

            var playerDistance = ploc.DistanceTo(bloc);
            ret.State = PlayerState.FindBall;
            if (tlocs != null)
            {
                foreach (var tloc in tlocs)
                {
                    if (playerDistance > tloc.DistanceTo(bloc))
                    {
                        ret.State = PlayerState.Free;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        private PlayerAIResult StateAttack(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            if (factor.IsFlagOn("PlayerState.Free"))
            {
                ret.State = PlayerState.Free;
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
                ret.State = PlayerState.Free;
                return ret;
            }
            else if (factor.IsFlagOn("PlayerState.Dribble"))
            {
                if (factor.IsFlagOn("TargetInfo.Type.Goal"))
                {
                    //패스할 데가 있나 확인
                    CourtPos ploc = new CourtPos();
                    factor.GetValue("PlayerLocation", ref ploc);

                    CourtPos rloc = new CourtPos();
                    factor.GetValue("RingLocation", ref rloc);

                    CourtPos[] tlocs;
                    factor.GetValues("TeammateLocation", out tlocs,false);

                    var distanceToRing = ploc.DistanceTo(rloc);
                    if (tlocs != null)
                    {
                        foreach (var tloc in tlocs)
                        {
                            var dis = distanceToRing - tloc.DistanceTo(rloc);
                            if (dis > 20)
                            {
                                ret.State = PlayerState.Pass;
                                return ret;
                            }
                        }
                    }

                    //슛이 가능한지 현재 위치 확인
                    if (this.GetShootingPoint(ploc, rloc) > 80)
                    {
                        //현재 상태를 슛상태로 변환
                        ret.State = PlayerState.Shoot;
                        return ret;
                    }
                    else
                    {
                        ret.State = PlayerState.Dribble;
                        return ret;
                    }
                }
                else
                {
                    throw new Exception { };
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

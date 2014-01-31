using System;
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
            var ret = new PlayerAIResult();
            if (factor.IsFlagOn("TeamState.LooseBall"))
            {
                Vector3f ploc;
                factor.GetVector("PlayerLocation", out ploc);

                Vector3f bloc;
                factor.GetVector("BallLocation", out bloc);

                Vector3f[] tlocs;
                factor.GetVectors("TeammateLocation", out tlocs);

                var playerDistance = ploc.Distance(bloc);
                ret.State = PlayerState.FindBall;

                foreach (var tloc in tlocs)
                {
                    if (playerDistance > tloc.Distance(bloc))
                    {
                        ret.State = PlayerState.Free;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return ret;
            }
            else if (factor.IsFlagOn("TeamState.Attack"))
            {
                if (factor.IsFlagOn("PlayerState.Free"))
                {
                    ret.State = PlayerState.Free;
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
                        Vector3f ploc;
                        factor.GetVector("PlayerLocation", out ploc);

                        Vector3f rloc;
                        factor.GetVector("RingLocation", out rloc);

                        Vector3f[] tlocs;
                        factor.GetVectors("TeammateLocation", out tlocs);

                        var playerDistance = ploc.Distance(rloc);
                        foreach (var tloc in tlocs)
                        {
                            var dis = playerDistance - tloc.Distance(rloc);
                            if (dis > 20)
                            {
                                ret.State = PlayerState.Pass;
                                return ret;
                            }
                        }

                        //슛이 가능한지 현재 위치 확인
                        if (factor.IsFlagOn("CanShoot"))
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
            else if (factor.IsFlagOn("TeamState.Defence"))
            {
                return this.StateDefence(factor);
            }
            else
            {
                throw new Exception { };
            }
        }

        private PlayerAIResult StateDefence(PropertyBag factor)
        {
            var ret = new PlayerAIResult();
            Vector3f v;
            if (factor.GetVector("TargetLocation", out v))
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

        private int GetShootingPoint(CourtPos playerpoint, CourtPos ringpoint)
        {
            //슛을 쏠지 말지 결정하는 팩터들을 수치화
            float distancefromRing = playerpoint.DistanceTo(ringpoint);

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

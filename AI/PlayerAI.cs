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

        public PlayerAIResult Determine(DetermineFactor factor)
        {
            var ret = new PlayerAIResult();
            if (factor.TeamState == TeamState.LooseBall)
            {
                var playerDistance = factor.PlayerLocation.DistanceTo(factor.BallLocation);

                ret.state = PlayerState.FindBall;
                foreach (var tloc in factor.TeammateLocations)
                {
                    if (playerDistance > tloc.DistanceTo(factor.BallLocation))
                    {
                        ret.state = PlayerState.Free;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return ret;
            }
            else if (factor.TeamState == TeamState.Attack)
            {
                if (factor.CurrentState == PlayerState.Free)
                {
                    ret.state = PlayerState.Free;
                    return ret;
                }
                else if (factor.CurrentState == PlayerState.FindBall)
                {
                    ret.state = PlayerState.Free;
                    return ret;
                }
                else if (factor.CurrentState == PlayerState.Dribble)
                {
                    if (factor.TargetInfo.TargetType == TargetInfo.Type.Goal)
                    {
                        //슛이 가능한지 현재 위치 확인
                        if (GetShootingPoint(factor.PlayerLocation, factor.TargetInfo.Position) > 80)
                        {
                            //현재 상태를 슛상태로 변환
                            ret.state = PlayerState.Shoot;
                            return ret;
                        }
                        else
                        {
                            ret.state = PlayerState.Dribble;
                            return ret;
                        }
                    }
                    else
                    {
                        throw new Exception { };
                    }
                }
                else if (factor.CurrentState == PlayerState.Shoot)
                {
                    ret.state = PlayerState.Rebound;
                    return ret;
                }
                else if (factor.CurrentState == PlayerState.Rebound)
                {
                    ret.state = PlayerState.Rebound;
                    return ret;
                }
                else
                {
                    throw new Exception { };
                }
            }
            else
            {
                throw new Exception { };
            }
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

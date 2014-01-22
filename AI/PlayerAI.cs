using System;
using System.Drawing;
using Physics;

using Core;

namespace AI
{
    public static class PlayerAI
    {
        private static LogHelper log = new LogHelper();
        //private Boolean hasBall = false;

        private static int GetShootingPoint(CourtPos playerpoint, CourtPos ringpoint)
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

        public static PlayerState Determine(DetermineFactor factor)
        {
            if (factor.TeamState == TeamState.LooseBall)
            {
                foreach (var playerposition in factor.TeammateLocations)
                {
                    //playerposition.DistanceTo(
                }
                return PlayerState.FindBall;
            }
            else if (factor.TeamState == TeamState.Attack)
            {
                if (factor.CurrentState == PlayerState.Free)
                {
                    return PlayerState.Free;
                }
                else if (factor.CurrentState == PlayerState.FindBall)
                {
                    return PlayerState.Free;
                }
                else if (factor.CurrentState == PlayerState.Dribble)
                {

                    if (factor.TargetInfo.TargetType == TargetInfo.Type.Goal)
                    {
                        //슛이 가능한지 현재 위치 확인
                        if (GetShootingPoint(factor.PlayerLocation, factor.TargetInfo.Position) > 80)
                        {
                            //현재 상태를 슛상태로 변환
                            return PlayerState.Shoot;
                        }
                        else
                        {
                            return PlayerState.Dribble;
                        }
                    }
                    else
                    {
                        throw new Exception { };
                    }
                }
                else if (factor.CurrentState == PlayerState.Shoot)
                {
                    //Rebound()
                    //리바운드
                    return PlayerState.Rebound;
                }
                else if (factor.CurrentState == PlayerState.Rebound)
                {
                    //Rebound()
                    //리바운드
                    return PlayerState.Rebound;
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

        public static void SetReporter(IReporter er)
        {
            log.SetReporter(er);
        }
    }
}

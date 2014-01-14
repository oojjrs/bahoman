using System;
using System.Drawing;
using Physics;

using Core;

namespace AI
{
    public static class PlayerAI
    {
        private static Point ringposition = new Point(550, 0);
        private static LogHelper log = new LogHelper();
        //private Boolean hasBall = false;

        private static int GetShootingPoint(Point playerpoint,Point ringpoint)
        {
            //슛을 쏠지 말지 결정하는 팩터들을 수치화
            float distancefromRing = PhysicsEngine.GetDistance(playerpoint,ringpoint);
            
            //일단 골대 근처에 있으면 100점으로 리턴
            if (40 > (int)distancefromRing)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public static PlayerState Determine(PlayerState currentstate, Point playerposition)
        {
            if (currentstate == PlayerState.Free)
            {
                return PlayerState.Free;
            } 
            else if (currentstate == PlayerState.Dribble)
            {
                //슛이 가능한지 현재 위치 확인
                if (GetShootingPoint(playerposition, ringposition) > 80)
                {
                    //현재 상태를 슛상태로 변환
                    return PlayerState.Shoot;
                }
                else
                {
                    //그냥 드리블 해서 골대로 이동
                    return PlayerState.Dribble;
                }
            }
            else if (currentstate == PlayerState.Shoot)
            {
                //Shooting()
                //슛상태일때는 슛하고 리바운드 혹은 수비 준비 해야지;
                float distancefromRing = PhysicsEngine.GetDistance(playerposition, ringposition);
                if (distancefromRing < 50)
                {
                    return PlayerState.Rebound;
                }
                else
                {
                    return PlayerState.Free;
                }
                
            }
            else if (currentstate == PlayerState.Rebound)
            {
                //Rebound()
                //리바운드
                throw new Exception { };
            }
            else if (currentstate == PlayerState.Free)
            {
                //Move
                //슛상태일때는 슛하고 리바운드 혹은 수비 준비 해야지;
                throw new Exception { };
            }
            else
            {
                throw new Exception{};
            }
        }

        public static void SetReporter(IReporter er)
        {
            log.SetReporter(er);
        }
    }
}

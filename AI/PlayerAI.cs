using System;
using System.Drawing;
using Physics;

namespace AI
{
    class PlayerAI
    {
        private Point ringpoint = new Point(50, 50);
        //private Boolean hasBall = false;
        public PlayerState GetCurrentPlayerState()
        {
            //공을 가지고 있으면
            return PlayerState.Dribble;
        }

        private int GetShootingPoint(Point playerpoint,Point ringpoint)
        {
            //슛을 쏠지 말지 결정하는 팩터들을 수치화
            float distancefromRing = PhysicsEngine.GetDistance(playerpoint,ringpoint);
            
            //일단 골대 근처에 있으면 100점으로 리턴
            if (10 > (int)distancefromRing)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public PlayerState Determine(State currentstate, Point playerposition)
        {
            if (GetCurrentPlayerState() == PlayerState.Dribble)
            {
                //슛이 가능한지 현재 위치 확인
                if (GetShootingPoint(playerposition, ringpoint) > 80)
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
            else if (GetCurrentPlayerState() == PlayerState.Shoot)
            {
                //Shooting()
                //슛상태일때는 슛하고 리바운드 혹은 수비 준비 해야지;
                float distancefromRing = PhysicsEngine.GetDistance(playerposition, ringpoint);
                if (distancefromRing < 10)
                {
                    return PlayerState.Rebound;
                }
                else
                {
                    return PlayerState.Free;
                }
                
            }
            else
            {
                throw new Exception{};
            }
        }
    }
}

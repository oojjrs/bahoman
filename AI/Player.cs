using System;

namespace AI
{
    class Player
    {
        //private Boolean hasBall = false;
        
        public PlayerState GetCurrentPlayerState()
        {
            //공을 가지고 있으면
            return PlayerState.Dribble;
        }

        //이거 위치 벡터 리턴하는 메소드 필요
        public Array GetPlayerPosition()
        {
            return new Array[0,1];
        }

        public int GetShootingPoint()
        {
            //슛을 쏠지 말지 결정하는 팩터들을 수치화
            //일단 골대 근처에 있으면 100점으로 리턴
            return 100;
        }

        public PlayerState Determine(State state, Team team, BasketballCourt basketballCourt)
        {
            if (GetCurrentPlayerState() == PlayerState.Dribble)
            {
                //슛이 가능한지 현재 위치 확인
                if (GetShootingPoint() > 80)
                {
                    //현재 상태를 슛상태로 변환
                    return PlayerState.Shoot;
                }
                else
                {
                    //그냥 드리블 해서 골대로 이동
                    //MoveToRing(can moveable)
                    return PlayerState.Dribble;
                }
            }
            else if (GetCurrentPlayerState() == PlayerState.Shoot)
            {
                //Shooting()
                //슛상태일때는 슛하고 리바운드 혹은 수비 준비 해야지;
                throw new Exception { };
            }
            else
            {
                throw new Exception{};
            }
        }
    }
}

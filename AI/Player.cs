using System;

namespace AI
{
    class Player
    {
        enum PlayerState
        {
            Dribbling,
            PreDribble,
            Stop,
            Free
        }

        enum States
        {
            Attack,
            Defence,

            FreeThrow,
            JumpBall,
            OutOfBound,
            StrategyTime
        }


        private Boolean hasBall = false;
        private PlayerState currentState = PlayerState.Free;

        public string Determine(State state, Team team, BasketballCourt basketballCourt)
        {
            //if (state.GetCurrentState() == States.Attack)
            //{
            //    if (hasBall)
            //    {
            //        if (currentState == PlayerState.Dribbling)
            //        {
            //            //드리블 상태
            //            //전술, 내 특성, 내 위치 확인하고 우리팀 위치, 수비 위치 확인하고 패스할지 슛할지, 계속 드리블해서 이동할지 결정
            //        }
            //        else if (currentState == PlayerState.PreDribble)
            //        {
            //            //드리블 하기 전 상태
            //            //전술, 내 특성, 내 위치 확인하고 우리팀 위치, 수비 위치 확인하고 패스할지 슛할지, 계속 드리블해서 이동할지 결정
            //        }
            //        else if (currentState == PlayerState.Stop)
            //        {
            //            //드리블을 마치고 정지한 상태
            //            //전술, 내 특성, 내 위치 확인하고 우리팀 위치, 수비 위치 확인하고 패스할지 슛할지, 계속 드리블해서 이동할지 결정
            //        }
            //        else
            //        {
            //            //에러 : 볼을 가졌는데 상태가 볼을 가진 상태가 아닌경우
            //        }
            //    }
            //    else
            //    {
            //        if (currentState == PlayerState.Free)
            //        {
            //            // 내 상태가 프리
            //            //전술, 내 특성, 내 위치 확인하고 우리팀 위치, 수비 위치 확인하고 특정 위치로 이동할지, 스크린을 해줄지, 가만히 있을지 결정
            //        }
            //    }
            //}
            return "";
        }
    }
}

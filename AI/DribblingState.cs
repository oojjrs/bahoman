using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    class DribblingState : State
    {
        public DribblingState()
        {

        }
        public override void Enter()
        {
            base.Enter();

        }

        public override void Excute()
        {
            base.Excute();
            //드리블 상태
            //전술, 내 특성, 내 위치 확인하고 우리팀 위치, 수비 위치 확인하고 패스할지 슛할지, 계속 드리블해서 이동할지 결정
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

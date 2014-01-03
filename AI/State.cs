using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public enum States
    {
        Attack,
        Defence,

        FreeThrow,
        JumpBall,
        OutOfBound,
        StrategyTime
    }

    class State
    {
        public State()
        {
        }

        public States GetCurrentState()
        {
            return States.Attack;
        }
    }
}

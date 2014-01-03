using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    class State
    {
        public State()
        {
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bball
{
    enum Position
    {
        Bench,
        PointGuard,
        ShootingGuard,
        SmallFoward,
        PowerFoward,
        Center,
    }

    public enum TeamState
    {
        Attack,
        Defence,
        StrategyTime,
        LooseBall
    }

    public enum GameState
    {
        JumpBall,
        OutOfBound,
        StrategyTime,
        FreeThrow
    }
}

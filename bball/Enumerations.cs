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

    enum AttackStrategy
    {
        MotionOffence
    }

    enum DefenceStrategy
    {
        ManToMan,
        Box23
    }

    enum TeamState
    {
        Attack,
        Defence,
        StrategyTime,
        LooseBall
    }

    enum GameState
    {
        JumpBall,
        OutOfBound,
        StrategyTime,
        FreeThrow
    }
}

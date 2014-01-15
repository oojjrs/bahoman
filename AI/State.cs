using System;

namespace AI
{
    public enum PlayerState
    {
        Dribble,
        Shoot,
        Pass,
        Rebound,
        Free,
        FindBall
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

    public class State
    {
        //public enum States
        //{
        //    Attack,
        //    Defence,
        //    FreeThrow,
        //    JumpBall,
        //    OutOfBound,
        //    StrategyTime
        //}
    }
}
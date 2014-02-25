using System;

namespace AI
{
    public enum PlayerState
    {
        Ready,
        Dribble,
        Shoot,
        Pass,
        Rebound,
        OffBall,
        FindBall,
        Move,
        CatchBall,
        Stand,
    }
    public enum StrategyState
    {
        Run,
        Stop,
        None
    }
    public enum BallState
    {
        Bounding,
        Dribbling,
        //Holding,
        //Intercepting,
        Passing,
        //Rolling,
        Shooting,
        //Stealing,
    }
}
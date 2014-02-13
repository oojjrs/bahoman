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
        Free,
        FindBall,
        Move,
        CatchBall,
        Stand,
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
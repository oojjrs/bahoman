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
        CatchBall
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
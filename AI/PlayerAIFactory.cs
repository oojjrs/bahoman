using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public static class PlayerAIFactory
    {
        public enum Type
        {
            //DebugMode,
            ExpertSystem,
            //Genetic,
            //Neural,
        }

        public static IPlayerAIType Create(Type t)
        {
            switch(t)
            {
                case Type.ExpertSystem:
                    return PlayerExpert.Instance;
            }
            throw new Exception("새 타입에 따른 핸들러 정의가 필요합니다");
        }
    }
}

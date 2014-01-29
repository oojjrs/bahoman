using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bball
{
    struct UID
    {
        public enum Type
        {
            Player,
            Team,
        }

        public static readonly UID Null = new UID(0);

        private static int seqPlayer = 0;
        private static int seqTeam = 0;

        private readonly int value;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.value == UID.Null.value)
                return false;
            return this.value == ((UID)obj).value;
        }

        public override int GetHashCode()
        {
            return value;
        }

        public static bool operator ==(UID l, UID r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(UID l, UID r)
        {
            return !l.Equals(r);
        }

        private UID(int v)
        {
            value = v;
        }

        public static UID Create(Type t)
        {
            switch(t)
            {
                case Type.Player:
                    return new UID(++seqPlayer);
                case Type.Team:
                    return new UID(0x10000000 + ++seqTeam);
                default:
                    throw new Exception("UID 새 타입에 대한 핸들링이 필요합니다");
            }
        }
    }
}

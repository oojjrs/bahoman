using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Renderer;
using AI;

namespace bball
{
    class PlayerPool
    {
        private Dictionary<UID, Player> players = new Dictionary<UID, Player>();

        public UID Add(string name, DateTime birthday, IImage image, IPlayerAIType ai, Position position)
        {
            var uid = UID.Create(UID.Type.Player);
            if (uid != UID.Null)
            {
                var pi = new PlayerInfo(uid);
                pi.Name = name;
                pi.Birthday = birthday;
                pi.Image = image;
                pi.AI = ai;
                pi.Position = position;
                players[uid] = new Player(pi);
            }
            return uid;
        }

        public Player Get(UID uid)
        {
            if (uid == UID.Null)
                return null;

            Player p;
            if (players.TryGetValue(uid, out p))
                return p;
            return null;
        }
    }
}
